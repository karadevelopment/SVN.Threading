using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SVN.Threading
{
    public class Thread
    {
        private static List<Thread> Instances { get; } = new List<Thread>();
        private static CancellationTokenSource CancellationTokenSource { get; } = new CancellationTokenSource();
        public static Action<Exception> Logger { get; set; }

        public double Time { get; private set; }
        public double DeltaTime { get; private set; }

        private bool Created { get; set; }
        private bool Enabled { get; set; }
        private bool Started { get; set; }

        public bool Active { get; set; }
        public bool Disposed { get; set; }

        public static bool Running
        {
            get => Thread.Instances.Any();
        }

        private bool CanCreate
        {
            get => !this.Created && this.Active;
        }

        private bool CanDispose
        {
            get => this.Disposed;
        }

        private bool CanEnable
        {
            get => this.Created && !this.Enabled && this.Active;
        }

        private bool CanDisable
        {
            get => this.Created && this.Enabled && !this.Active;
        }

        private bool CanStart
        {
            get => this.Created && this.Enabled && !this.Started;
        }

        private bool CanUpdate
        {
            get => this.Created && this.Enabled && this.Started;
        }

        static Thread()
        {
            Task.Factory.StartNew(() =>
            {
                var stopwatch = new Stopwatch();

                while (true)
                {
                    stopwatch.Restart();

                    lock (Thread.Instances)
                    {
                        foreach (var thread in Thread.Instances.Where(x => x.CanCreate))
                        {
                            try
                            {
                                thread.Create();
                            }
                            catch (Exception e)
                            {
                                Thread.Logger?.Invoke(e);
                            }
                        }
                        foreach (var thread in Thread.Instances.Where(x => x.CanEnable))
                        {
                            try
                            {
                                thread.Enable();
                            }
                            catch (Exception e)
                            {
                                Thread.Logger?.Invoke(e);
                            }
                        }
                        foreach (var thread in Thread.Instances.Where(x => x.CanStart))
                        {
                            try
                            {
                                thread.Start();
                            }
                            catch (Exception e)
                            {
                                Thread.Logger?.Invoke(e);
                            }
                        }
                        foreach (var thread in Thread.Instances.Where(x => x.CanUpdate))
                        {
                            try
                            {
                                thread.Update();
                            }
                            catch (Exception e)
                            {
                                Thread.Logger?.Invoke(e);
                            }
                        }
                        foreach (var thread in Thread.Instances.Where(x => x.CanDisable))
                        {
                            try
                            {
                                thread.Disable();
                            }
                            catch (Exception e)
                            {
                                Thread.Logger?.Invoke(e);
                            }
                        }
                        foreach (var thread in Thread.Instances.Where(x => x.CanDispose))
                        {
                            try
                            {
                                thread.Dispose();
                            }
                            catch (Exception e)
                            {
                                Thread.Logger?.Invoke(e);
                            }
                        }

                        var deltaTime = stopwatch.Elapsed.TotalSeconds;
                        foreach (var thread in Thread.Instances.Where(x => x.Enabled))
                        {
                            thread.Time += deltaTime;
                        }
                        foreach (var thread in Thread.Instances)
                        {
                            thread.DeltaTime = deltaTime;
                        }
                    }
                }
            }, Thread.CancellationTokenSource.Token);
        }

        protected Thread(bool active = true)
        {
            this.Active = active;
            Thread.Instances.Add(this);
        }

        public static T Register<T>()
            where T : Thread, new()
        {
            var thread = new T();

            return thread;
        }

        public static void Sleep(TimeSpan timeSpan)
        {
            System.Threading.Thread.Sleep(timeSpan);
        }

        public static void Abort()
        {
            Thread.CancellationTokenSource.Cancel();
        }

        private void Create()
        {
            this.OnCreate();
            this.Created = true;
        }

        private void Dispose()
        {
            this.OnDispose();
            Thread.Instances.Remove(this);
        }

        private void Enable()
        {
            this.OnEnable();
            this.Enabled = true;
        }

        private void Disable()
        {
            this.OnDisable();
            this.Enabled = false;
        }

        private void Start()
        {
            this.OnStart();
            this.Started = true;
        }

        private void Update()
        {
            this.OnUpdate();
        }

        protected virtual void OnCreate()
        {
        }

        protected virtual void OnDispose()
        {
        }

        protected virtual void OnEnable()
        {
        }

        protected virtual void OnDisable()
        {
        }

        protected virtual void OnStart()
        {
        }

        protected virtual void OnUpdate()
        {
        }
    }
}