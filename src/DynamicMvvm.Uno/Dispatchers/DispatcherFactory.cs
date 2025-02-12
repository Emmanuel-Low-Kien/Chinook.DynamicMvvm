﻿using Windows.UI.Xaml;

namespace Chinook.DynamicMvvm
{
	/// <summary>
	/// This is the default implementation of <see cref="IDispatcherFactory"/>.
	/// It uses the <see cref="CoreDispatcherDispatcher"/> implementation by default.
	/// </summary>
	[Preserve(AllMembers = true)]
	public class DispatcherFactory : IDispatcherFactory
	{
		private readonly CreateDispatcher _createDispatcher;

		/// <summary>
		/// Creates a new instance of <see cref="DispatcherFactory"/>.
		/// </summary>
		/// <param name="createDispatcher">
		/// The optional method to use to generate the <see cref="IDispatcher"/>.
		/// When not provided, a method using the <see cref="CoreDispatcherDispatcher"/> implementation is used.
		/// </param>
		public DispatcherFactory(CreateDispatcher createDispatcher = null)
		{
			_createDispatcher = createDispatcher ?? CreateFromCoreDispatcher;
		}

		/// <inheritdoc/>
		public IDispatcher Create(object view)
		{
			return _createDispatcher(view);
		}

		private IDispatcher CreateFromCoreDispatcher(object view)
		{
			return new CoreDispatcherDispatcher((FrameworkElement)view);
		}
	}

	/// <summary>
	/// This deletage is used to create an <see cref="IDispatcher"/> from a native view object.
	/// </summary>
	/// <param name="view">The native view object.</param>
	/// <returns>A <see cref="IDispatcher"/> instance.</returns>
	public delegate IDispatcher CreateDispatcher(object view);
}
