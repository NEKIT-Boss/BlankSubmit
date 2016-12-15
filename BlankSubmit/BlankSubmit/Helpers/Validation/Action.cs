// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="Action.cs" company="XLabs Team">
//     Copyright (c) XLabs Team. All rights reserved.
// </copyright>
// <summary>
//       This project is licensed under the Apache 2.0 license
//       https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/LICENSE
//       
//       XLabs is a open source project that aims to provide a powerfull and cross 
//       platform set of controls tailored to work with Xamarin Forms.
// </summary>
// ***********************************************************************
// 

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace BlankSubmit.Helpers.Validation
{
	/// <summary>
	///     Defines an action to be taken after validation
	/// </summary>
	/// Element created at 07/11/2014,4:03 AM by Charles
	public class Action : BindableObject
	{
		#region Static Fields

		/// <summary>Definition for <see cref="Setters"/></summary>
		/// Element created at 08/11/2014,4:01 PM by Charles
		public BindableProperty SettersProperty =
			BindableProperty.Create(nameof(Setters),
				typeof(PropertySetters), typeof(Action), default(PropertySetters));

		/// <summary>Property Definition for <see cref="Element" /></summary>
		/// Element created at 07/11/2014,6:15 AM by Charles
		public BindableProperty ElementProperty =
			BindableProperty.Create(nameof(Element),
				typeof(BindableObject), typeof(Action), default(BindableObject));

		/// <summary>Property Defintion for <see cref="InvalidValue" /></summary>
		/// Element created at 07/11/2014,6:16 AM by Charles
		public BindableProperty InvalidValueProperty =
			BindableProperty.Create(nameof(InvalidValue), typeof(object), typeof(Action), null);

		/// <summary>Property definition for <see cref="Property" /></summary>
		/// Element created at 07/11/2014,6:15 AM by Charles
		public BindableProperty PropertyProperty =
			BindableProperty.Create(nameof(Property), typeof(string), typeof(Action), default(string));

		/// <summary>Property Definition for <see cref="ValidValue" /> </summary>
		/// Element created at 07/11/2014,6:15 AM by Charles
		public BindableProperty ValidValueProperty =
			BindableProperty.Create(nameof(ValidValue), typeof(object), typeof(Action), null);

		private static readonly Dictionary<Type, TypeConverter> Converters = new Dictionary
			<Type, TypeConverter>
			{
				{ typeof(Color), new ColorTypeConverter() },
				{ typeof(Rectangle), new BoundsTypeConverter() },
				{ typeof(Constraint), new ConstraintTypeConverter() },
				{ typeof(Font), new FontTypeConverter() },
				{ typeof(GridLength), new GridLengthTypeConverter() },
				{ typeof(ImageSource), new XLabs.Forms.Controls.ImageSourceConverter() },
				{ typeof(Keyboard), new KeyboardTypeConverter() },
				{ typeof(Point), new PointTypeConverter() },
				{ typeof(Thickness), new ThicknessTypeConverter() },
				{ typeof(Uri), new UriTypeConverter() },
				{ typeof(WebViewSource), new WebViewSourceTypeConverter() }
			};

		#endregion

		#region Fields

		private PropertyInfo _pi;

		#endregion

		#region Public Properties

		/// <summary>Gets or sets the setters.</summary>
		/// <value>The setters to apply to the Element identified by the Action element.</value>
		/// Element created at 08/11/2014,4:02 PM by Charles
		public PropertySetters Setters
		{
			get { return (PropertySetters)GetValue(SettersProperty); }
			set { SetValue(SettersProperty,value);}
		}
		/// <summary>Gets or sets the element to be modified.</summary>
		/// <value>The element.</value>
		/// Element created at 07/11/2014,6:16 AM by Charles
		public BindableObject Element
		{
			get { return (BindableObject)GetValue(ElementProperty); }
			set { SetValue(ElementProperty, value); }
		}

		/// <summary>Gets or sets the invalid value.</summary>
		/// <value>The valud to be applied to the property when the RuleSet is invalid value.</value>
		/// Element created at 07/11/2014,6:17 AM by Charles
		public object InvalidValue
		{
			get { return GetValue(InvalidValueProperty); }
			set { SetValue(InvalidValueProperty, value); }
		}

		/// <summary>Gets or sets the property.</summary>
		/// <value>The property to be modified.</value>
		/// Element created at 07/11/2014,6:16 AM by Charles
		public string Property
		{
			get { return (string)GetValue(PropertyProperty); }
			set { SetValue(PropertyProperty, value); }
		}

		/// <summary>Gets or sets the valid value.</summary>
		/// <value>The value to be applied to the property when the RuleSet is valid.</value>
		/// Element created at 07/11/2014,6:16 AM by Charles
		public object ValidValue
		{
			get { return GetValue(ValidValueProperty); }
			set { SetValue(ValidValueProperty, value); }
		}

		#endregion

		#region Properties

		/// <summary>Gets the property information.</summary>
		/// <value>The property information.</value>
		/// Element created at 07/11/2014,6:18 AM by Charles
		/// <exception cref="XLabs.Exceptions.PropertyNotFoundException"></exception>
		protected virtual PropertyInfo PropertyInfo
		{
			get { return _pi ?? (_pi = GetPropertyInfo(Property, Element.GetType())); }
		}

		#endregion

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="Action"/> class.
		/// </summary>
		/// Element created at 08/11/2014,4:03 PM by Charles
		public Action()
		{
			Setters= new PropertySetters();
		}
		/// <summary>
		///     Applies the result of the validation, valid if result is true, invalid otherwise
		/// </summary>
		/// <param name="result">Flag indicating the state of the RuleSet</param>
		/// <param name="sourceElement">If this action has no target target this 
		/// instead (The source from the rule)</param>
		/// Element created at 07/11/2014,6:17 AM by Charles
		internal void ApplyResult(bool result,BindableObject sourceElement=null)
		{
			try
			{
				var target = sourceElement ?? Element;
				var targetType = target.GetType();
				//Process the property defined inline (if any)
				if (!string.IsNullOrEmpty(Property))
				{
					var pi = GetPropertyInfo(Property, targetType);
					if (pi != null)
						AppplyValueToProperty(target, pi, result ? ValidValue : InvalidValue);
				}
				//Process the setters collection if present
				if (Setters == null || !Setters.Any())  return; 
				foreach (var s in Setters)
				{
					var propinfo = GetPropertyInfo(s.Property, targetType);
					if(propinfo != null)
						AppplyValueToProperty(target,propinfo,result ? s.ValidValue : s.InvalidValue);
				}
			}
			catch (Exception ex)
			{
				throw new InvalidCastException(
					string.Format("Could not convert {0} to {1}",
						result ? ValidValue : InvalidValue,
						PropertyInfo.PropertyType.Name),ex);
			}
		}

		private static void AppplyValueToProperty(BindableObject target, PropertyInfo info, object value)
		{
			var finalvalue = TryConvert(value, info.PropertyType);
			info.SetValue(target, finalvalue);
		}


		private static object TryConvert(object value, Type targetType)
		{
			object retval;
			if (Converters.ContainsKey(targetType)
				&& Converters[targetType].CanConvertFrom(value.GetType()))
			{
				retval = Converters[targetType].ConvertFrom(CultureInfo.InvariantCulture, value);
			}
			else
			{
				retval = Convert.ChangeType(value, targetType);
			}
			return retval;
		}

		private PropertyInfo GetPropertyInfo(string property, Type type)
		{
			List<PropertyInfo> allprops = type.GetRuntimeProperties().ToList();
			PropertyInfo propinfo =
				allprops.FirstOrDefault(
					x => string.Compare(x.Name, property, StringComparison.OrdinalIgnoreCase) == 0);
			return propinfo;
		}

		#endregion
	}
}
