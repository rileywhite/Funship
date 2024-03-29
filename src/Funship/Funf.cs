﻿/***************************************************************************/
// Copyright 2019 Riley White
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
/***************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;

namespace Funship
{
    /// <summary>
    /// Represents a functional-style function.
    /// </summary>
    /// <remarks>
    /// A basic <see cref="Funf"/> is a container for one of the various <see cref="Func{TResult}"/>
    /// types, of which there are 17 variants (0 to 16 arguments). See the overloads of <see cref="funf(Func{dynamic})"/>
    /// for a full list of functional delegate types that can be wrapped.
    ///
    /// They can get more advanced.
    ///
    /// For example, when a <see cref="Funf"/> is called via <see cref="call(Funf, dynamic[])"/>,
    /// depending on the number of arguments passed and the <see cref="arity"/>, it may be partially called,
    /// fully called, or even overflowed, returning its own return value along with the extra arguments.
    /// See documentation for <see cref="call(Funf, dynamic[])"/> for more information.
    ///
    /// A <see cref="Funf"/> may also capture arguments without attempting to execute via a call to
    /// <see cref="capture(Funf, dynamic[])"/>. This creates a new <see cref="Funf"/>
    /// that encloses the passed arguments such that they will be used at the time the <see cref="Funf"/>
    /// is called.
    ///
    /// Additionally, calling <see cref="compose(Funf, Funf)"/> allows you to create a new
    /// function <c>h</c> that composes given functions <c>f</c> and <c>g</c> such that
    /// upon being called, <c>h</c> applies arguments to <c>f</c> until <c>f</c> can be executed,
    /// at which time any results from <c>f</c> are applied to <c>g</c> along with any remaining
    /// arguments that were not used by <c>f</c>.
    ///
    /// Any time a <see cref="Funf"/> is passed as an argument to <see cref="call(Funf, dynamic[])"/>
    /// or <see cref="capture(Funf, dynamic[])"/>, the returned <see cref="Funf"/> will compose the
    /// argument Funf together with the original.
    /// </remarks>
    /// <seealso cref="call(Funf, dynamic[])"/>
    /// <seealso cref="capture(Funf, dynamic[])"/>
    /// <seealso cref="compose(Funf, Funf)"/>
    public partial interface Funf
    {
        /// <summary>
        /// Gets the number of arguments the <see cref="Funf"/> needs before it can be fully called.
        /// </summary>
        /// <remarks>
        /// This may be a negative number indicating that the arguments are overflowing. If such a
        /// <see cref="Funf"/> is called, then the return value will be an <see cref="IEnumerable{dynamic}"/>
        /// with the return value as the first item followed by the unneeded arguments.
        /// </remarks>
        int arity { get; }

        #region Factories

        /// <summary>
        /// Creates a new <see cref="Funf"/> from a given
        /// <see cref="Func{TResult}"/>
        /// </summary>
        /// <param name="func">Function that will be executed upon successful call</param>
        /// <returns>New <see cref="Funf"/></returns>
        /// <example>
        /// var f = Funf(() => 0);
        /// var x = call(f);        // x = 0
        /// </example>
        public static Funf funf(Func<dynamic> func) => new WFunf(func);

        /// <summary>
        /// Creates a new <see cref="Funf"/> from a given
        /// <see cref="Func{T1, TResult}"/>
        /// </summary>
        /// <param name="func">Function that will be executed upon successful call</param>
        /// <returns>New <see cref="Funf"/></returns>
        /// <example>
        /// var f = Funf((arg) => arg);
        /// var x = call(f, 1);        // x = 1
        /// </example>
        public static Funf funf(Func<dynamic, dynamic> func) => new WFunf(func);

        /// <summary>
        /// Creates a new <see cref="Funf"/> from a given
        /// <see cref="Func{T1, T2, TResult}"/>
        /// </summary>
        /// <param name="func">Function that will be executed upon successful call</param>
        /// <returns>New <see cref="Funf"/></returns>
        /// <example>
        /// var f = Funf((a1, a2) => a1 + a2);
        /// var x = call(f, 1, 1);        // x = 2
        /// </example>
        public static Funf funf(Func<dynamic, dynamic, dynamic> func) => new WFunf(func);

        /// <summary>
        /// Creates a new <see cref="Funf"/> from a given
        /// <see cref="Func{T1, T2, T3, TResult}"/>
        /// </summary>
        /// <param name="func">Function that will be executed upon successful call</param>
        /// <returns>New <see cref="Funf"/></returns>
        /// <example>
        /// var f = Funf((a1, a2, a3) => a1 + a2 + a3);
        /// var x = call(f, 1, 1, 1);        // x = 3
        /// </example>
        public static Funf funf(Func<dynamic, dynamic, dynamic, dynamic> func) => new WFunf(func);

        /// <summary>
        /// Creates a new <see cref="Funf"/> from a given
        /// <see cref="Func{T1, T2, T3, T4, TResult}"/>
        /// </summary>
        /// <param name="func">Function that will be executed upon successful call</param>
        /// <returns>New <see cref="Funf"/></returns>
        /// <example>
        /// var f = Funf((a1, a2, a3, a4) => a1 + a2 + a3 + a4);
        /// var x = call(f, 1, 1, 1, 1);        // x = 4
        /// </example>
        public static Funf funf(Func<dynamic, dynamic, dynamic, dynamic, dynamic> func) => new WFunf(func);

        /// <summary>
        /// Creates a new <see cref="Funf"/> from a given
        /// <see cref="Func{T1, T2, T3, T4, T5, TResult}"/>
        /// </summary>
        /// <param name="func">Function that will be executed upon successful call</param>
        /// <returns>New <see cref="Funf"/></returns>
        /// <example>
        /// var f = Funf((a1, a2, a3, a4, a5) => a1 + a2 + a3 + a4 + a5);
        /// var x = call(f, 1, 1, 1, 1, 1);        // x = 5
        /// </example>
        public static Funf funf(Func<dynamic, dynamic, dynamic, dynamic, dynamic, dynamic> func) => new WFunf(func);

        /// <summary>
        /// Creates a new <see cref="Funf"/> from a given
        /// <see cref="Func{T1, T2, T3, T4, T5, T6, TResult}"/>
        /// </summary>
        /// <param name="func">Function that will be executed upon successful call</param>
        /// <returns>New <see cref="Funf"/></returns>
        /// <example>
        /// var f = Funf((a1, a2, a3, a4, a5, a6) => a1 + a2 + a3 + a4 + a5 + a6);
        /// var x = call(f, 1, 1, 1, 1, 1, 1);        // x = 6
        /// </example>
        public static Funf funf(Func<dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic> func) => new WFunf(func);

        /// <summary>
        /// Creates a new <see cref="Funf"/> from a given
        /// <see cref="Func{T1, T2, T3, T4, T5, T6, T7, TResult}"/>
        /// </summary>
        /// <param name="func">Function that will be executed upon successful call</param>
        /// <returns>New <see cref="Funf"/></returns>
        /// <example>
        /// var f = Funf((a1, a2, a3, a4, a5, a6, a7) => a1 + a2 + a3 + a4 + a5 + a6 + a7);
        /// var x = call(f, 1, 1, 1, 1, 1, 1, 1);        // x = 7
        /// </example>
        public static Funf funf(Func<dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic> func) => new WFunf(func);

        /// <summary>
        /// Creates a new <see cref="Funf"/> from a given
        /// <see cref="Func{T1, T2, T3, T4, T5, T6, T7, T8, TResult}"/>
        /// </summary>
        /// <param name="func">Function that will be executed upon successful call</param>
        /// <returns>New <see cref="Funf"/></returns>
        /// <example>
        /// var f = Funf((a1, a2, a3, a4, a5, a6, a7, a8) => a1 + a2 + a3 + a4 + a5 + a6 + a7 + a8);
        /// var x = call(f, 1, 1, 1, 1, 1, 1, 1, 1);        // x = 8
        /// </example>
        public static Funf funf(Func<dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic> func) => new WFunf(func);

        /// <summary>
        /// Creates a new <see cref="Funf"/> from a given
        /// <see cref="Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult}"/>
        /// </summary>
        /// <param name="func">Function that will be executed upon successful call</param>
        /// <returns>New <see cref="Funf"/></returns>
        /// <example>
        /// var f = Funf((a1, a2, a3, a4, a5, a6, a7, a8, a9) => a1 + a2 + a3 + a4 + a5 + a6 + a7 + a8 + a9);
        /// var x = call(f, 1, 1, 1, 1, 1, 1, 1, 1, 1);        // x = 9
        /// </example>
        public static Funf funf(Func<dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic> func) => new WFunf(func);

        /// <summary>
        /// Creates a new <see cref="Funf"/> from a given
        /// <see cref="Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult}"/>
        /// </summary>
        /// <param name="func">Function that will be executed upon successful call</param>
        /// <returns>New <see cref="Funf"/></returns>
        /// <example>
        /// var f = Funf((a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) => a1 + a2 + a3 + a4 + a5 + a6 + a7 + a8 + a9 + a10);
        /// var x = call(f, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);        // x = 10
        /// </example>
        public static Funf funf(Func<dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic> func) => new WFunf(func);

        /// <summary>
        /// Creates a new <see cref="Funf"/> from a given
        /// <see cref="Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult}"/>
        /// </summary>
        /// <param name="func">Function that will be executed upon successful call</param>
        /// <returns>New <see cref="Funf"/></returns>
        /// <example>
        /// var f = Funf((a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) => a1 + a2 + a3 + a4 + a5 + a6 + a7 + a8 + a9 + a10 + a11);
        /// var x = call(f, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);        // x = 11
        /// </example>
        public static Funf funf(Func<dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic> func) => new WFunf(func);

        /// <summary>
        /// Creates a new <see cref="Funf"/> from a given
        /// <see cref="Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult}"/>
        /// </summary>
        /// <param name="func">Function that will be executed upon successful call</param>
        /// <returns>New <see cref="Funf"/></returns>
        /// <example>
        /// var f = Funf((a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) => a1 + a2 + a3 + a4 + a5 + a6 + a7 + a8 + a9 + a10 + a11 + a12);
        /// var x = call(f, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);        // x = 12
        /// </example>
        public static Funf funf(Func<dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic> func) => new WFunf(func);

        /// <summary>
        /// Creates a new <see cref="Funf"/> from a given
        /// <see cref="Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult}"/>
        /// </summary>
        /// <param name="func">Function that will be executed upon successful call</param>
        /// <returns>New <see cref="Funf"/></returns>
        /// <example>
        /// var f = Funf((a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) => a1 + a2 + a3 + a4 + a5 + a6 + a7 + a8 + a9 + a10 + a11 + a12 + a13);
        /// var x = call(f, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);        // x = 13
        /// </example>
        public static Funf funf(Func<dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic> func) => new WFunf(func);

        /// <summary>
        /// Creates a new <see cref="Funf"/> from a given
        /// <see cref="Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult}"/>
        /// </summary>
        /// <param name="func">Function that will be executed upon successful call</param>
        /// <returns>New <see cref="Funf"/></returns>
        /// <example>
        /// var f = Funf((a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) => a1 + a2 + a3 + a4 + a5 + a6 + a7 + a8 + a9 + a10 + a11 + a12 + a13 + a14);
        /// var x = call(f, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);        // x = 14
        /// </example>
        public static Funf funf(Func<dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic> func) => new WFunf(func);

        /// <summary>
        /// Creates a new <see cref="Funf"/> from a given
        /// <see cref="Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult}"/>
        /// </summary>
        /// <param name="func">Function that will be executed upon successful call</param>
        /// <returns>New <see cref="Funf"/></returns>
        /// <example>
        /// var f = Funf((a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) => a1 + a2 + a3 + a4 + a5 + a6 + a7 + a8 + a9 + a10 + a11 + a12 + a13 + a14 + a15);
        /// var x = call(f, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);        // x = 15
        /// </example>
        public static Funf funf(Func<dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic> func) => new WFunf(func);

        /// <summary>
        /// Creates a new <see cref="Funf"/> from a given
        /// <see cref="Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult}"/>
        /// </summary>
        /// <param name="func">Function that will be executed upon successful call</param>
        /// <returns>New <see cref="Funf"/></returns>
        /// <example>
        /// var f = Funf((a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) => a1 + a2 + a3 + a4 + a5 + a6 + a7 + a8 + a9 + a10 + a11 + a12 + a13 + a14 + a15 + a16);
        /// var x = call(f, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);        // x = 16
        /// </example>
        public static Funf funf(Func<dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic> func) => new WFunf(func);

        #endregion

        /// <summary>
        /// Simple wrapped dotnet function
        /// </summary>
        private readonly struct WFunf : Funf
        {
            #region Constructors

            public WFunf(Func<dynamic> func)
            {
                this.Func = func;
                this.arity = 0;
            }

            public WFunf(Func<dynamic, dynamic> func)
            {
                this.Func = func ?? (x => x);
                this.arity = 1;
            }

            public WFunf(Func<dynamic, dynamic, dynamic> func)
            {
                this.Func = func ?? ((x1, x2) => (x1, x2));
                this.arity = 2;
            }

            public WFunf(Func<dynamic, dynamic, dynamic, dynamic> func)
            {
                this.Func = func ?? ((x1, x2, x3) => (x1, x2, x3));
                this.arity = 3;
            }

            public WFunf(Func<dynamic, dynamic, dynamic, dynamic, dynamic> func)
            {
                this.Func = func ?? ((x1, x2, x3, x4) => (x1, x2, x3, x4));
                this.arity = 4;
            }

            public WFunf(Func<dynamic, dynamic, dynamic, dynamic, dynamic, dynamic> func)
            {
                this.Func = func ?? ((x1, x2, x3, x4, x5) => (x1, x2, x3, x4, x5));
                this.arity = 5;
            }

            public WFunf(Func<dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic> func)
            {
                this.Func = func ?? ((x1, x2, x3, x4, x5, x6) => (x1, x2, x3, x4, x5, x6));
                this.arity = 6;
            }

            public WFunf(Func<dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic> func)
            {
                this.Func = func ?? ((x1, x2, x3, x4, x5, x6, x7) => (x1, x2, x3, x4, x5, x6, x7));
                this.arity = 7;
            }

            public WFunf(Func<dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic> func)
            {
                this.Func = func ?? ((x1, x2, x3, x4, x5, x6, x7, x8) => (x1, x2, x3, x4, x5, x6, x7, x8));
                this.arity = 8;
            }

            public WFunf(Func<dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic> func)
            {
                this.Func = func ?? ((x1, x2, x3, x4, x5, x6, x7, x8, x9) => (x1, x2, x3, x4, x5, x6, x7, x8, x9));
                this.arity = 9;
            }

            public WFunf(Func<dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic> func)
            {
                this.Func = func ?? ((x1, x2, x3, x4, x5, x6, x7, x8, x9, x10) => (x1, x2, x3, x4, x5, x6, x7, x8, x9, x10));
                this.arity = 10;
            }

            public WFunf(Func<dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic> func)
            {
                this.Func = func ?? ((x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11) => (x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11));
                this.arity = 11;
            }

            public WFunf(Func<dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic> func)
            {
                this.Func = func ?? ((x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12) => (x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12));
                this.arity = 12;
            }

            public WFunf(Func<dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic> func)
            {
                this.Func = func ?? ((x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13) => (x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13));
                this.arity = 13;
            }

            public WFunf(Func<dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic> func)
            {
                this.Func = func ?? ((x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13, x14) => (x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13, x14));
                this.arity = 14;
            }

            public WFunf(Func<dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic> func)
            {
                this.Func = func ?? ((x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13, x14, x15) => (x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13, x14, x15));
                this.arity = 15;
            }

            public WFunf(Func<dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic> func)
            {
                this.Func = func ?? ((x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13, x14, x15, x16) => (x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13, x14, x15, x16));
                this.arity = 16;
            }

            #endregion

            private dynamic Func { get; }

            public int arity { get; }
            private IEnumerable<dynamic> args => new dynamic[0];

            public dynamic invoke_func(IEnumerable<dynamic> args) => invoke_func(args.ToArray());
            public dynamic invoke_func(params dynamic[] args) => args.Length switch
            {
                int l when l > arity => Enumerable.Prepend(args.Skip(arity), this.Func.DynamicInvoke(args.Take(arity).ToArray())),
                int l when l == arity => this.Func.DynamicInvoke(args),
                _ => capture(this, args),
            };
        }

        /// <summary>
        /// A function that encloses some captured args
        /// </summary>
        private readonly struct CapFunf : Funf
        {
            public CapFunf(Funf f, IEnumerable<dynamic> args, int arity)
            {
                this.f = f;
                this.args = args;
                this.arity = arity;
            }

            private Funf f { get; }
            private IEnumerable<dynamic> args { get; }
            public int arity { get; }

            public dynamic collect_args_and_call(params dynamic[] moreArgs) => this.collect_args_and_call(moreArgs.AsEnumerable());
            public dynamic collect_args_and_call(IEnumerable<dynamic> moreArgs) => call(f, this.args.Concat(moreArgs));
        }

        /// <summary>
        /// Composed function that, when executed, will pass its outcome to another function
        /// </summary>
        private readonly struct CompFunf : Funf
        {
            public CompFunf(Funf g, Funf f, int arity, IEnumerable<dynamic> args)
            {
                this.f = f;
                this.g = g;
                this.args = args;
                this.arity = arity;
            }

            private Funf f { get; }
            private Funf g { get; }
            private IEnumerable<dynamic> args { get; }
            public int arity { get; }


            public dynamic collect_args_and_call(params dynamic[] moreArgs) => this.collect_args_and_call(moreArgs.AsEnumerable());
            public dynamic collect_args_and_call(IEnumerable<dynamic> moreArgs)
            {
                var allArgs = this.args.Concat(moreArgs).ToArray();
                return allArgs.Length switch
                {
                    var l when l < f.arity => new CompFunf(g, f, f.arity + g.arity - l, allArgs),
                    _ => call(g, call(f, allArgs))
                };
            }
        }
    }
}
