﻿using System;
using System.Text;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;

namespace Chinook.DynamicMvvm
{
	/// <summary>
	/// This is a default implementation of <see cref="IViewModelView"/>.
	/// </summary>
	public class ViewModelView : IViewModelView
	{
		private readonly FrameworkElement _frameworkElement;

		/// <summary>
		/// Initializes a new instance of the <see cref="ViewModelView"/> class.
		/// </summary>
		/// <param name="frameworkElement"><see cref="FrameworkElement"/></param>
		public ViewModelView(FrameworkElement frameworkElement)
		{
			_frameworkElement = frameworkElement ?? throw new ArgumentNullException(nameof(frameworkElement));

			_frameworkElement.Loaded += OnLoaded;
			_frameworkElement.Unloaded += OnUnloaded;
		}

		/// <inheritdoc />
		public bool GetHasDispatcherAccess() => _frameworkElement.Dispatcher.HasThreadAccess;

		/// <inheritdoc />
		public event EventHandler Loaded;

		/// <inheritdoc />
		public event EventHandler Unloaded;

		/// <inheritdoc />
		public async Task ExecuteOnDispatcher(CancellationToken ct, Action action)
		{
			if (ct.IsCancellationRequested)
			{
				this.Log().LogDebug($"Cancelled 'ExecuteOnDispatcher' because of the cancellation token.");
				return;
			}

			await _frameworkElement.Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
			{
				try
				{
					if (ct.IsCancellationRequested)
					{
						this.Log().LogDebug($"Cancelled 'ExecuteOnDispatcher' because of the cancellation token.");
						return;
					}

					action();
				}
				catch (Exception e)
				{
					this.Log().LogError(e, "Failed 'ExecuteOnDispatcher'.");
				}
			});
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			Loaded?.Invoke(sender, EventArgs.Empty);
		}

		private void OnUnloaded(object sender, RoutedEventArgs e)
		{
			Unloaded?.Invoke(sender, EventArgs.Empty);
		}
	}
}
