﻿// This file is part of Hangfire. Copyright © 2013-2014 Hangfire OÜ.
// 
// Hangfire is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as 
// published by the Free Software Foundation, either version 3 
// of the License, or any later version.
// 
// Hangfire is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public 
// License along with Hangfire. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Hangfire.Annotations;
using Hangfire.States;

namespace Hangfire
{
    /// <summary>
    /// Provides static methods for creating <i>fire-and-forget</i>, <i>delayed</i>
    /// jobs and <i>continuations</i> as well as re-queue and delete existing
    /// background jobs.
    /// </summary>
    /// 
    /// <remarks>
    /// <para>This class is a wrapper for the <see cref="IBackgroundJobClient"/> 
    /// interface and its default implementation, <see cref="BackgroundJobClient"/>
    /// class, that was created for the most simple scenarios. Please consider 
    /// using the types above in real world applications.</para>
    /// <para>This class also contains undocumented constructor and instance 
    /// members. They are hidden to not to confuse new users. You can freely 
    /// use them in low-level API.</para>
    /// </remarks>
    /// 
    /// <seealso cref="IBackgroundJobClient"/>
    /// <seealso cref="BackgroundJobClient"/>
    /// 
    /// <threadsafety static="true" instance="false" />
    public partial class BackgroundJob
    {
        private static readonly Lazy<IBackgroundJobClient> CachedClient 
            = new Lazy<IBackgroundJobClient>(static () => new BackgroundJobClient(), LazyThreadSafetyMode.PublicationOnly); 

        private static readonly Func<IBackgroundJobClient> DefaultFactory
            = static () => CachedClient.Value;

        private static Func<IBackgroundJobClient> _clientFactory;
        private static readonly object ClientFactoryLock = new object();

        //如果传了客户端工厂，就使用客户端工厂，否则就使用默认工厂
        internal static Func<IBackgroundJobClient> ClientFactory
        {
            get
            {
                lock (ClientFactoryLock)
                {
                    return _clientFactory ?? DefaultFactory;
                }
            }
            set
            {
                lock (ClientFactoryLock)
                {
                    _clientFactory = value;
                }
            }
        }

        /// <summary>
        /// Creates a new fire-and-forget job based on a given method call expression.
        /// </summary>
        /// <param name="methodCall">Method call expression that will be marshalled to a server.</param>
        /// <returns>Unique identifier of a background job.</returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="methodCall"/> is <see langword="null"/>.
        /// </exception>
        /// 
        /// <seealso cref="EnqueuedState"/>
        /// <seealso cref="O:Hangfire.IBackgroundJobClient.Enqueue"/>
        public static string Enqueue([NotNull, InstantHandle] Expression<Action> methodCall)
        {
            return ClientFactory().Enqueue(methodCall);
        }

        /// <summary>
        /// Creates a new fire-and-forget job based on a given method call expression and places it
        /// to the specified queue.
        /// </summary>
        /// <param name="queue">Default queue for the background job.</param>
        /// <param name="methodCall">Method call expression that will be marshalled to a server.</param>
        /// <returns>Unique identifier of a background job.</returns>
        ///
        /// <exception cref="ArgumentNullException">
        /// <paramref name="methodCall"/> is <see langword="null"/>.
        /// </exception>
        ///
        /// <seealso cref="EnqueuedState"/>
        /// <seealso cref="O:Hangfire.IBackgroundJobClient.Enqueue"/>
        public static string Enqueue([NotNull] string queue, [NotNull, InstantHandle] Expression<Action> methodCall)
        {
            return ClientFactory().Enqueue(queue, methodCall);
        }

        /// <summary>
        /// Creates a new fire-and-forget job based on a given method call expression.
        /// </summary>
        /// <param name="methodCall">Method call expression that will be marshalled to a server.</param>
        /// <returns>Unique identifier of a background job.</returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="methodCall"/> is <see langword="null"/>.
        /// </exception>
        /// 
        /// <seealso cref="EnqueuedState"/>
        /// <seealso cref="O:Hangfire.IBackgroundJobClient.Enqueue"/>
        public static string Enqueue([NotNull, InstantHandle] Expression<Func<Task>> methodCall)
        {
            return ClientFactory().Enqueue(methodCall);
        }

        /// <summary>
        /// Creates a new fire-and-forget job based on a given method call expression and places it
        /// to the specified queue.
        /// </summary>
        /// <param name="queue">Default queue for the background job.</param>
        /// <param name="methodCall">Method call expression that will be marshalled to a server.</param>
        /// <returns>Unique identifier of a background job.</returns>
        ///
        /// <exception cref="ArgumentNullException">
        /// <paramref name="methodCall"/> is <see langword="null"/>.
        /// </exception>
        ///
        /// <seealso cref="EnqueuedState"/>
        /// <seealso cref="O:Hangfire.IBackgroundJobClient.Enqueue"/>
        public static string Enqueue([NotNull] string queue, [NotNull, InstantHandle] Expression<Func<Task>> methodCall)
        {
            return ClientFactory().Enqueue(queue, methodCall);
        }

        /// <summary>
        /// Creates a new fire-and-forget job based on a given method call expression.
        /// </summary>
        /// <param name="methodCall">Method call expression that will be marshalled to a server.</param>
        /// <returns>Unique identifier of a background job.</returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="methodCall"/> is <see langword="null"/>.
        /// </exception>
        /// 
        /// <seealso cref="EnqueuedState"/>
        /// <seealso cref="O:Hangfire.IBackgroundJobClient.Enqueue"/>
        public static string Enqueue<T>([NotNull, InstantHandle] Expression<Action<T>> methodCall)
        {
            return ClientFactory().Enqueue(methodCall);
        }

        /// <summary>
        /// Creates a new fire-and-forget job based on a given method call expression and places it
        /// to the specified queue.
        /// </summary>
        /// <param name="queue">Default queue for the background job.</param>
        /// <param name="methodCall">Method call expression that will be marshalled to a server.</param>
        /// <returns>Unique identifier of a background job.</returns>
        ///
        /// <exception cref="ArgumentNullException">
        /// <paramref name="methodCall"/> is <see langword="null"/>.
        /// </exception>
        ///
        /// <seealso cref="EnqueuedState"/>
        /// <seealso cref="O:Hangfire.IBackgroundJobClient.Enqueue"/>
        public static string Enqueue<T>([NotNull] string queue, [NotNull, InstantHandle] Expression<Action<T>> methodCall)
        {
            return ClientFactory().Enqueue(queue, methodCall);
        }

        /// <summary>
        /// Creates a new fire-and-forget job based on a given method call expression.
        /// </summary>
        /// <param name="methodCall">Method call expression that will be marshalled to a server.</param>
        /// <returns>Unique identifier of a background job.</returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="methodCall"/> is <see langword="null"/>.
        /// </exception>
        /// 
        /// <seealso cref="EnqueuedState"/>
        /// <seealso cref="O:Hangfire.IBackgroundJobClient.Enqueue"/>
        public static string Enqueue<T>([NotNull, InstantHandle] Expression<Func<T, Task>> methodCall)
        {
            return ClientFactory().Enqueue(methodCall);
        }

        /// <summary>
        /// Creates a new fire-and-forget job based on a given method call expression and places it
        /// to the specified queue.
        /// </summary>
        /// <param name="queue">Default queue for the background job.</param>
        /// <param name="methodCall">Method call expression that will be marshalled to a server.</param>
        /// <returns>Unique identifier of a background job.</returns>
        ///
        /// <exception cref="ArgumentNullException">
        /// <paramref name="methodCall"/> is <see langword="null"/>.
        /// </exception>
        ///
        /// <seealso cref="EnqueuedState"/>
        /// <seealso cref="O:Hangfire.IBackgroundJobClient.Enqueue"/>
        public static string Enqueue<T>([NotNull] string queue, [NotNull, InstantHandle] Expression<Func<T, Task>> methodCall)
        {
            return ClientFactory().Enqueue(queue, methodCall);
        }

        /// <summary>
        /// Creates a new background job based on a specified method
        /// call expression and schedules it to be enqueued after a given delay.
        /// </summary>
        /// 
        /// <param name="methodCall">Instance method call expression that will be marshalled to the Server.</param>
        /// <param name="delay">Delay, after which the job will be enqueued.</param>
        /// <returns>Unique identifier of the created job.</returns>
        public static string Schedule(
            [NotNull, InstantHandle] Expression<Action> methodCall, 
            TimeSpan delay)
        {
            return ClientFactory().Schedule(methodCall, delay);
        }

        /// <summary>
        /// Creates a new background job based on a specified method call expression and schedules it
        /// to be enqueued to the specified queue after a given delay.
        /// </summary>
        ///
        /// <param name="queue">Default queue for the background job.</param>
        /// <param name="methodCall">Instance method call expression that will be marshalled to the Server.</param>
        /// <param name="delay">Delay, after which the job will be enqueued.</param>
        /// <returns>Unique identifier of the created job.</returns>
        public static string Schedule(
            [NotNull] string queue,
            [NotNull, InstantHandle] Expression<Action> methodCall,
            TimeSpan delay)
        {
            return ClientFactory().Schedule(queue, methodCall, delay);
        }

        /// <summary>
        /// Creates a new background job based on a specified method
        /// call expression and schedules it to be enqueued after a given delay.
        /// </summary>
        /// 
        /// <param name="methodCall">Instance method call expression that will be marshalled to the Server.</param>
        /// <param name="delay">Delay, after which the job will be enqueued.</param>
        /// <returns>Unique identifier of the created job.</returns>
        public static string Schedule(
            [NotNull, InstantHandle] Expression<Func<Task>> methodCall,
            TimeSpan delay)
        {
            return ClientFactory().Schedule(methodCall, delay);
        }

        /// <summary>
        /// Creates a new background job based on a specified method call expression and schedules it
        /// to be enqueued to the specified queue after a given delay.
        /// </summary>
        ///
        /// <param name="queue">Default queue for the background job.</param>
        /// <param name="methodCall">Instance method call expression that will be marshalled to the Server.</param>
        /// <param name="delay">Delay, after which the job will be enqueued.</param>
        /// <returns>Unique identifier of the created job.</returns>
        public static string Schedule(
            [NotNull] string queue,
            [NotNull, InstantHandle] Expression<Func<Task>> methodCall,
            TimeSpan delay)
        {
            return ClientFactory().Schedule(queue, methodCall, delay);
        }

        /// <summary>
        /// Creates a new background job based on a specified method call expression
        /// and schedules it to be enqueued at the given moment of time.
        /// </summary>
        /// 
        /// <param name="methodCall">Method call expression that will be marshalled to the Server.</param>
        /// <param name="enqueueAt">The moment of time at which the job will be enqueued.</param>
        /// <returns>Unique identifier of a created job.</returns>
        public static string Schedule(
            [NotNull, InstantHandle] Expression<Action> methodCall, 
            DateTimeOffset enqueueAt)
        {
            return ClientFactory().Schedule(methodCall, enqueueAt);
        }

        /// <summary>
        /// Creates a new background job based on a specified method call expression and schedules it
        /// to be enqueued to the specified queue at the given moment of time.
        /// </summary>
        ///
        /// <param name="queue">Default queue for the background job.</param>
        /// <param name="methodCall">Method call expression that will be marshalled to the Server.</param>
        /// <param name="enqueueAt">The moment of time at which the job will be enqueued.</param>
        /// <returns>Unique identifier of a created job.</returns>
        public static string Schedule(
            [NotNull] string queue,
            [NotNull, InstantHandle] Expression<Action> methodCall,
            DateTimeOffset enqueueAt)
        {
            return ClientFactory().Schedule(queue, methodCall, enqueueAt);
        }

        /// <summary>
        /// Creates a new background job based on a specified method call expression
        /// and schedules it to be enqueued at the given moment of time.
        /// </summary>
        /// 
        /// <param name="methodCall">Method call expression that will be marshalled to the Server.</param>
        /// <param name="enqueueAt">The moment of time at which the job will be enqueued.</param>
        /// <returns>Unique identifier of a created job.</returns>
        public static string Schedule(
            [NotNull, InstantHandle] Expression<Func<Task>> methodCall,
            DateTimeOffset enqueueAt)
        {
            return ClientFactory().Schedule(methodCall, enqueueAt);
        }

        /// <summary>
        /// Creates a new background job based on a specified method call expression and schedules it
        /// to be enqueued to the specified queue at the given moment of time.
        /// </summary>
        ///
        /// <param name="queue">Default queue for the background job.</param>
        /// <param name="methodCall">Method call expression that will be marshalled to the Server.</param>
        /// <param name="enqueueAt">The moment of time at which the job will be enqueued.</param>
        /// <returns>Unique identifier of a created job.</returns>
        public static string Schedule(
            [NotNull] string queue,
            [NotNull, InstantHandle] Expression<Func<Task>> methodCall,
            DateTimeOffset enqueueAt)
        {
            return ClientFactory().Schedule(queue, methodCall, enqueueAt);
        }

        /// <summary>
        /// Creates a new background job based on a specified instance method
        /// call expression and schedules it to be enqueued after a given delay.
        /// </summary>
        /// 
        /// <typeparam name="T">Type whose method will be invoked during job processing.</typeparam>
        /// <param name="methodCall">Instance method call expression that will be marshalled to the Server.</param>
        /// <param name="delay">Delay, after which the job will be enqueued.</param>
        /// <returns>Unique identifier of the created job.</returns>
        public static string Schedule<T>(
            [NotNull, InstantHandle] Expression<Action<T>> methodCall,
            TimeSpan delay)
        {
            return ClientFactory().Schedule(methodCall, delay);
        }

        /// <summary>
        /// Creates a new background job based on a specified instance method call expression and schedules
        /// it to be enqueued to the specified queue after a given delay.
        /// </summary>
        ///
        /// <typeparam name="T">Type whose method will be invoked during job processing.</typeparam>
        /// <param name="queue">Default queue for the background job.</param>
        /// <param name="methodCall">Instance method call expression that will be marshalled to the Server.</param>
        /// <param name="delay">Delay, after which the job will be enqueued.</param>
        /// <returns>Unique identifier of the created job.</returns>
        public static string Schedule<T>(
            [NotNull] string queue,
            [NotNull, InstantHandle] Expression<Action<T>> methodCall,
            TimeSpan delay)
        {
            return ClientFactory().Schedule(queue, methodCall, delay);
        }

        /// <summary>
        /// Creates a new background job based on a specified instance method
        /// call expression and schedules it to be enqueued after a given delay.
        /// </summary>
        /// 
        /// <typeparam name="T">Type whose method will be invoked during job processing.</typeparam>
        /// <param name="methodCall">Instance method call expression that will be marshalled to the Server.</param>
        /// <param name="delay">Delay, after which the job will be enqueued.</param>
        /// <returns>Unique identifier of the created job.</returns>
        public static string Schedule<T>(
            [NotNull, InstantHandle] Expression<Func<T, Task>> methodCall,
            TimeSpan delay)
        {
            return ClientFactory().Schedule(methodCall, delay);
        }

        /// <summary>
        /// Creates a new background job based on a specified instance method call expression and schedules
        /// it to be enqueued to the specified queue after a given delay.
        /// </summary>
        ///
        /// <typeparam name="T">Type whose method will be invoked during job processing.</typeparam>
        /// <param name="queue">Default queue for the background job.</param>
        /// <param name="methodCall">Instance method call expression that will be marshalled to the Server.</param>
        /// <param name="delay">Delay, after which the job will be enqueued.</param>
        /// <returns>Unique identifier of the created job.</returns>
        public static string Schedule<T>(
            [NotNull] string queue,
            [NotNull, InstantHandle] Expression<Func<T, Task>> methodCall,
            TimeSpan delay)
        {
            return ClientFactory().Schedule(queue, methodCall, delay);
        }

        /// <summary>
        /// Creates a new background job based on a specified method call expression
        /// and schedules it to be enqueued at the given moment of time.
        /// </summary>
        /// 
        /// <typeparam name="T">The type whose method will be invoked during the job processing.</typeparam>
        /// <param name="methodCall">Method call expression that will be marshalled to the Server.</param>
        /// <param name="enqueueAt">The moment of time at which the job will be enqueued.</param>
        /// <returns>Unique identifier of a created job.</returns>
        public static string Schedule<T>(
            [NotNull, InstantHandle] Expression<Action<T>> methodCall, 
            DateTimeOffset enqueueAt)
        {
            return ClientFactory().Schedule(methodCall, enqueueAt);
        }

        /// <summary>
        /// Creates a new background job based on a specified method call expression and schedules it
        /// to be enqueued to the specified queue at the given moment of time.
        /// </summary>
        ///
        /// <typeparam name="T">The type whose method will be invoked during the job processing.</typeparam>
        /// <param name="queue">Default queue for the background job.</param>
        /// <param name="methodCall">Method call expression that will be marshalled to the Server.</param>
        /// <param name="enqueueAt">The moment of time at which the job will be enqueued.</param>
        /// <returns>Unique identifier of a created job.</returns>
        public static string Schedule<T>(
            [NotNull] string queue,
            [NotNull, InstantHandle] Expression<Action<T>> methodCall,
            DateTimeOffset enqueueAt)
        {
            return ClientFactory().Schedule(queue, methodCall, enqueueAt);
        }

        /// <summary>
        /// Creates a new background job based on a specified method call expression
        /// and schedules it to be enqueued at the given moment of time.
        /// </summary>
        /// 
        /// <typeparam name="T">The type whose method will be invoked during the job processing.</typeparam>
        /// <param name="methodCall">Method call expression that will be marshalled to the Server.</param>
        /// <param name="enqueueAt">The moment of time at which the job will be enqueued.</param>
        /// <returns>Unique identifier of a created job.</returns>
        public static string Schedule<T>(
            [NotNull, InstantHandle] Expression<Func<T, Task>> methodCall,
            DateTimeOffset enqueueAt)
        {
            return ClientFactory().Schedule(methodCall, enqueueAt);
        }

        /// <summary>
        /// Creates a new background job based on a specified method call expression and schedules it
        /// to be enqueued to the specified queue at the given moment of time.
        /// </summary>
        ///
        /// <typeparam name="T">The type whose method will be invoked during the job processing.</typeparam>
        /// <param name="queue">Default queue for the background job.</param>
        /// <param name="methodCall">Method call expression that will be marshalled to the Server.</param>
        /// <param name="enqueueAt">The moment of time at which the job will be enqueued.</param>
        /// <returns>Unique identifier of a created job.</returns>
        public static string Schedule<T>(
            [NotNull] string queue,
            [NotNull, InstantHandle] Expression<Func<T, Task>> methodCall,
            DateTimeOffset enqueueAt)
        {
            return ClientFactory().Schedule(queue, methodCall, enqueueAt);
        }

        /// <summary>
        /// Changes state of a job with the specified <paramref name="jobId"/>
        /// to the <see cref="DeletedState"/>. 
        /// <seealso cref="BackgroundJobClientExtensions.Delete(IBackgroundJobClient, string)"/>
        /// </summary>
        /// 
        /// <param name="jobId">An identifier, that will be used to find a job.</param>
        /// <returns>True on a successful state transition, false otherwise.</returns>
        public static bool Delete([NotNull] string jobId)
        {
            return ClientFactory().Delete(jobId);
        }

        /// <summary>
        /// Changes state of a job with the specified <paramref name="jobId"/>
        /// to the <see cref="DeletedState"/>. State change is only performed
        /// if current job state is equal to the <paramref name="fromState"/> value.
        /// <seealso cref="BackgroundJobClientExtensions.Delete(IBackgroundJobClient, string, string)"/>
        /// </summary>
        /// 
        /// <param name="jobId">Identifier of job, whose state is being changed.</param>
        /// <param name="fromState">Current state assertion, or null if unneeded.</param>
        /// <returns>True, if state change succeeded, otherwise false.</returns>
        public static bool Delete([NotNull] string jobId, [CanBeNull] string fromState)
        {
            return ClientFactory().Delete(jobId, fromState);
        }
        
        /// <summary>
        /// Changes state of a job with the specified <paramref name="jobId"/>
        /// to the <see cref="EnqueuedState"/>.
        /// </summary>
        /// 
        /// <param name="jobId">Identifier of job, whose state is being changed.</param>
        /// <returns>True, if state change succeeded, otherwise false.</returns>
        public static bool Requeue([NotNull] string jobId)
        {
            return ClientFactory().Requeue(jobId);
        }

        /// <summary>
        /// Changes state of a job with the specified <paramref name="jobId"/>
        /// to the <see cref="EnqueuedState"/>. If <paramref name="fromState"/> value 
        /// is not null, state change will be performed only if the current state name 
        /// of a job equal to the given value.
        /// </summary>
        /// 
        /// <param name="jobId">Identifier of job, whose state is being changed.</param>
        /// <param name="fromState">Current state assertion, or null if unneeded.</param>
        /// <returns>True, if state change succeeded, otherwise false.</returns>
        public static bool Requeue([NotNull] string jobId, [CanBeNull] string fromState)
        {
            return ClientFactory().Requeue(jobId, fromState);
        }

        /// <summary>
        /// Changes state of a job with the specified <paramref name="jobId"/>
        /// to the <see cref="ScheduledState"/>.
        /// </summary>
        ///
        /// <param name="jobId">Identifier of job, whose state is being changed.</param>
        /// <param name="delay">Delay, after which the job will be scheduled.</param>
        /// <returns>True, if state change succeeded, otherwise false.</returns>
        public static bool Reschedule([NotNull] string jobId, TimeSpan delay)
        {
            return ClientFactory().Reschedule(jobId, delay);
        }

        /// <summary>
        /// Changes state of a job with the specified <paramref name="jobId"/>
        /// to the <see cref="ScheduledState"/>. If <paramref name="fromState"/> value
        /// is not null, state change will be performed only if the current state name
        /// of a job equal to the given value.
        /// </summary>
        ///
        /// <param name="jobId">Identifier of job, whose state is being changed.</param>
        /// <param name="delay">Delay, after which the job will be scheduled.</param>
        /// <param name="fromState">Current state assertion, or null if unneeded.</param>
        /// <returns>True, if state change succeeded, otherwise false.</returns>
        public static bool Reschedule([NotNull] string jobId, TimeSpan delay, [CanBeNull] string fromState)
        {
            return ClientFactory().Reschedule(jobId, delay, fromState);
        }

        /// <summary>
        /// Changes state of a job with the specified <paramref name="jobId"/>
        /// to the <see cref="ScheduledState"/>.
        /// </summary>
        ///
        /// <param name="jobId">Identifier of job, whose state is being changed.</param>
        /// <param name="enqueueAt">The moment of time at which the job will be rescheduled.</param>
        /// <returns>True, if state change succeeded, otherwise false.</returns>
        public static bool Reschedule([NotNull] string jobId, DateTimeOffset enqueueAt)
        {
            return ClientFactory().Reschedule(jobId, enqueueAt);
        }

        /// <summary>
        /// Changes state of a job with the specified <paramref name="jobId"/>
        /// to the <see cref="ScheduledState"/>. If <paramref name="fromState"/> value
        /// is not null, state change will be performed only if the current state name
        /// of a job equal to the given value.
        /// </summary>
        ///
        /// <param name="jobId">Identifier of job, whose state is being changed.</param>
        /// <param name="enqueueAt">The moment of time at which the job will be rescheduled.</param>
        /// <param name="fromState">Current state assertion, or null if unneeded.</param>
        /// <returns>True, if state change succeeded, otherwise false.</returns>
        public static bool Reschedule([NotNull] string jobId, DateTimeOffset enqueueAt, [CanBeNull] string fromState)
        {
            return ClientFactory().Reschedule(jobId, enqueueAt, fromState);
        }

        /// <summary>
        /// Creates a new background job that will wait for a successful completion
        /// of another background job to be enqueued.
        /// </summary>
        /// <param name="parentId">Identifier of a background job to wait completion for.</param>
        /// <param name="methodCall">Method call expression that will be marshalled to a server.</param>
        /// <returns>Unique identifier of a created job.</returns>
        [Obsolete("Deprecated for clarity, please use ContinueJobWith method with the same arguments. Will be removed in 2.0.0.")]
        public static string ContinueWith(
            [NotNull] string parentId, 
            [NotNull, InstantHandle] Expression<Action> methodCall)
        {
            return ContinueJobWith(parentId, methodCall);
        }

        /// <summary>
        /// Creates a new background job that will wait for a successful completion 
        /// of another background job to be enqueued.
        /// </summary>
        /// <param name="parentId">Identifier of a background job to wait completion for.</param>
        /// <param name="methodCall">Method call expression that will be marshalled to a server.</param>
        /// <returns>Unique identifier of a created job.</returns>
        public static string ContinueJobWith(
            [NotNull] string parentId,
            [NotNull, InstantHandle] Expression<Action> methodCall)
        {
            return ClientFactory().ContinueJobWith(parentId, methodCall);
        }

        /// <summary>
        /// Creates a new background job that will wait for a successful completion 
        /// of another background job to be enqueued.
        /// </summary>
        /// <param name="parentId">Identifier of a background job to wait completion for.</param>
        /// <param name="methodCall">Method call expression that will be marshalled to a server.</param>
        /// <returns>Unique identifier of a created job.</returns>
        [Obsolete("Deprecated for clarity, please use ContinueJobWith method with the same arguments. Will be removed in 2.0.0.")]
        public static string ContinueWith<T>(
            [NotNull] string parentId, 
            [NotNull, InstantHandle] Expression<Action<T>> methodCall)
        {
            return ContinueJobWith(parentId, methodCall);
        }

        /// <summary>
        /// Creates a new background job that will wait for a successful completion 
        /// of another background job to be enqueued.
        /// </summary>
        /// <param name="parentId">Identifier of a background job to wait completion for.</param>
        /// <param name="methodCall">Method call expression that will be marshalled to a server.</param>
        /// <returns>Unique identifier of a created job.</returns>
        public static string ContinueJobWith<T>(
            [NotNull] string parentId,
            [NotNull, InstantHandle] Expression<Action<T>> methodCall)
        {
            return ClientFactory().ContinueJobWith(parentId, methodCall);
        }

        /// <summary>
        /// Creates a new background job that will wait for another background job to be enqueued.
        /// </summary>
        /// <param name="parentId">Identifier of a background job to wait completion for.</param>
        /// <param name="methodCall">Method call expression that will be marshalled to a server.</param>
        /// <param name="options">Continuation options.</param>
        /// <returns>Unique identifier of a created job.</returns>
        [Obsolete("Deprecated for clarity, please use ContinueJobWith method with the same arguments. Will be removed in 2.0.0.")]
        public static string ContinueWith(
            [NotNull] string parentId, 
            [NotNull, InstantHandle] Expression<Action> methodCall, 
            JobContinuationOptions options)
        {
            return ContinueJobWith(parentId, methodCall, options);
        }

        /// <summary>
        /// Creates a new background job that will wait for another background job to be enqueued.
        /// </summary>
        /// <param name="parentId">Identifier of a background job to wait completion for.</param>
        /// <param name="methodCall">Method call expression that will be marshalled to a server.</param>
        /// <param name="options">Continuation options.</param>
        /// <returns>Unique identifier of a created job.</returns>
        public static string ContinueJobWith(
            [NotNull] string parentId,
            [NotNull, InstantHandle] Expression<Action> methodCall,
            JobContinuationOptions options)
        {
            return ClientFactory().ContinueJobWith(parentId, methodCall, options);
        }

        /// <summary>
        /// Creates a new background job that will wait for another background job to be enqueued to the
        /// specified queue.
        /// </summary>
        /// <param name="parentId">Identifier of a background job to wait completion for.</param>
        /// <param name="queue">Default queue for the continuation.</param>
        /// <param name="methodCall">Method call expression that will be marshalled to a server.</param>
        /// <param name="options">Continuation options.</param>
        /// <returns>Unique identifier of a created job.</returns>
        public static string ContinueJobWith(
            [NotNull] string parentId,
            [NotNull] string queue,
            [NotNull, InstantHandle] Expression<Action> methodCall,
            JobContinuationOptions options = JobContinuationOptions.OnlyOnSucceededState)
        {
            return ClientFactory().ContinueJobWith(parentId, queue, methodCall, options: options);
        }

        /// <summary>
        /// Creates a new background job that will wait for another background job to be enqueued.
        /// </summary>
        /// <param name="parentId">Identifier of a background job to wait completion for.</param>
        /// <param name="methodCall">Method call expression that will be marshalled to a server.</param>
        /// <param name="options">Continuation options. By default, 
        /// <see cref="JobContinuationOptions.OnlyOnSucceededState"/> is used.</param>
        /// <returns>Unique identifier of a created job.</returns>
        [Obsolete("Deprecated for clarity, please use ContinueJobWith method with the same arguments. Will be removed in 2.0.0.")]
        public static string ContinueWith(
            [NotNull] string parentId,
            [NotNull, InstantHandle] Expression<Func<Task>> methodCall,
            JobContinuationOptions options = JobContinuationOptions.OnlyOnSucceededState)
        {
            return ContinueJobWith(parentId, methodCall, options);
        }

        /// <summary>
        /// Creates a new background job that will wait for another background job to be enqueued.
        /// </summary>
        /// <param name="parentId">Identifier of a background job to wait completion for.</param>
        /// <param name="methodCall">Method call expression that will be marshalled to a server.</param>
        /// <param name="options">Continuation options. By default, 
        /// <see cref="JobContinuationOptions.OnlyOnSucceededState"/> is used.</param>
        /// <returns>Unique identifier of a created job.</returns>
        public static string ContinueJobWith(
            [NotNull] string parentId,
            [NotNull, InstantHandle] Expression<Func<Task>> methodCall,
            JobContinuationOptions options = JobContinuationOptions.OnlyOnSucceededState)
        {
            return ClientFactory().ContinueJobWith(parentId, methodCall, options: options);
        }

        /// <summary>
        /// Creates a new background job that will wait for another background job to be enqueued to the
        /// specified queue.
        /// </summary>
        /// <param name="parentId">Identifier of a background job to wait completion for.</param>
        /// <param name="queue">Default queue for the continuation.</param>
        /// <param name="methodCall">Method call expression that will be marshalled to a server.</param>
        /// <param name="options">Continuation options. By default,
        /// <see cref="JobContinuationOptions.OnlyOnSucceededState"/> is used.</param>
        /// <returns>Unique identifier of a created job.</returns>
        public static string ContinueJobWith(
            [NotNull] string parentId,
            [NotNull] string queue,
            [NotNull, InstantHandle] Expression<Func<Task>> methodCall,
            JobContinuationOptions options = JobContinuationOptions.OnlyOnSucceededState)
        {
            return ClientFactory().ContinueJobWith(parentId, queue, methodCall, options: options);
        }

        /// <summary>
        /// Creates a new background job that will wait for another background job to be enqueued.
        /// </summary>
        /// <param name="parentId">Identifier of a background job to wait completion for.</param>
        /// <param name="methodCall">Method call expression that will be marshalled to a server.</param>
        /// <param name="options">Continuation options.</param>
        /// <returns>Unique identifier of a created job.</returns>
        [Obsolete("Deprecated for clarity, please use ContinueJobWith method with the same arguments. Will be removed in 2.0.0.")]
        public static string ContinueWith<T>(
            [NotNull] string parentId, 
            [NotNull, InstantHandle] Expression<Action<T>> methodCall, 
            JobContinuationOptions options)
        {
            return ContinueJobWith(parentId, methodCall, options);
        }

        /// <summary>
        /// Creates a new background job that will wait for another background job to be enqueued.
        /// </summary>
        /// <param name="parentId">Identifier of a background job to wait completion for.</param>
        /// <param name="methodCall">Method call expression that will be marshalled to a server.</param>
        /// <param name="options">Continuation options.</param>
        /// <returns>Unique identifier of a created job.</returns>
        public static string ContinueJobWith<T>(
            [NotNull] string parentId,
            [NotNull, InstantHandle] Expression<Action<T>> methodCall,
            JobContinuationOptions options)
        {
            return ClientFactory().ContinueJobWith(parentId, methodCall, options);
        }

        /// <summary>
        /// Creates a new background job that will wait for another background job to be enqueued to the
        /// specified queue.
        /// </summary>
        /// <param name="parentId">Identifier of a background job to wait completion for.</param>
        /// <param name="queue">Default queue for the continuation.</param>
        /// <param name="methodCall">Method call expression that will be marshalled to a server.</param>
        /// <param name="options">Continuation options.</param>
        /// <returns>Unique identifier of a created job.</returns>
        public static string ContinueJobWith<T>(
            [NotNull] string parentId,
            [NotNull] string queue,
            [NotNull, InstantHandle] Expression<Action<T>> methodCall,
            JobContinuationOptions options = JobContinuationOptions.OnlyOnSucceededState)
        {
            return ClientFactory().ContinueJobWith(parentId, queue, methodCall, options: options);
        }

        /// <summary>
        /// Creates a new background job that will wait for another background job to be enqueued.
        /// </summary>
        /// <param name="parentId">Identifier of a background job to wait completion for.</param>
        /// <param name="methodCall">Method call expression that will be marshalled to a server.</param>
        /// <param name="options">Continuation options. By default, 
        /// <see cref="JobContinuationOptions.OnlyOnSucceededState"/> is used.</param>
        /// <returns>Unique identifier of a created job.</returns>
        [Obsolete("Deprecated for clarity, please use ContinueJobWith method with the same arguments. Will be removed in 2.0.0.")]
        public static string ContinueWith<T>(
            [NotNull] string parentId,
            [NotNull, InstantHandle] Expression<Func<T, Task>> methodCall,
            JobContinuationOptions options = JobContinuationOptions.OnlyOnSucceededState)
        {
            return ContinueJobWith(parentId, methodCall, options);
        }

        /// <summary>
        /// Creates a new background job that will wait for another background job to be enqueued.
        /// </summary>
        /// <param name="parentId">Identifier of a background job to wait completion for.</param>
        /// <param name="methodCall">Method call expression that will be marshalled to a server.</param>
        /// <param name="options">Continuation options. By default, 
        /// <see cref="JobContinuationOptions.OnlyOnSucceededState"/> is used.</param>
        /// <returns>Unique identifier of a created job.</returns>
        public static string ContinueJobWith<T>(
            [NotNull] string parentId,
            [NotNull, InstantHandle] Expression<Func<T, Task>> methodCall,
            JobContinuationOptions options = JobContinuationOptions.OnlyOnSucceededState)
        {
            return ClientFactory().ContinueJobWith(parentId, methodCall, options: options);
        }

        /// <summary>
        /// Creates a new background job that will wait for another background job to be enqueued to the
        /// specified queue.
        /// </summary>
        /// <param name="parentId">Identifier of a background job to wait completion for.</param>
        /// <param name="queue">Default queue for the continuation.</param>
        /// <param name="methodCall">Method call expression that will be marshalled to a server.</param>
        /// <param name="options">Continuation options. By default,
        /// <see cref="JobContinuationOptions.OnlyOnSucceededState"/> is used.</param>
        /// <returns>Unique identifier of a created job.</returns>
        public static string ContinueJobWith<T>(
            [NotNull] string parentId,
            [NotNull] string queue,
            [NotNull, InstantHandle] Expression<Func<T, Task>> methodCall,
            JobContinuationOptions options = JobContinuationOptions.OnlyOnSucceededState)
        {
            return ClientFactory().ContinueJobWith(parentId, queue, methodCall, options: options);
        }
    }
}
