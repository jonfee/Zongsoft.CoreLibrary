﻿/*
 * Authors:
 *   钟峰(Popeye Zhong) <zongsoft@gmail.com>
 *
 * Copyright (C) 2003-2015 Zongsoft Corporation <http://www.zongsoft.com>
 *
 * This file is part of Zongsoft.CoreLibrary.
 *
 * Zongsoft.CoreLibrary is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * Zongsoft.CoreLibrary is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * Lesser General Public License for more details.
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with Zongsoft.CoreLibrary; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA
 */

using System;
using System.ComponentModel;

namespace Zongsoft.Security.Membership
{
	[AttributeUsage((AttributeTargets.Class | AttributeTargets.Method), AllowMultiple = false, Inherited = true)]
	public class AuthorizationAttribute : Attribute
	{
		#region 成员变量
		private string _schemaId;
		private string _actionId;
		private AuthorizationMode _mode;
		private Type _validatorType;
		private ICredentialValidator _validator;
		#endregion

		#region 构造函数
		public AuthorizationAttribute()
		{
			_actionId = string.Empty;
			_schemaId = string.Empty;

			_mode = AuthorizationMode.Identity;
		}

		public AuthorizationAttribute(bool disabled)
		{
			_actionId = string.Empty;
			_schemaId = string.Empty;

			_mode = disabled ? AuthorizationMode.Disabled : AuthorizationMode.Identity;
		}

		public AuthorizationAttribute(string schemaId) : this(schemaId, null)
		{
		}

		public AuthorizationAttribute(string schemaId, string actionId)
		{
			_actionId = actionId ?? string.Empty;
			_schemaId = schemaId ?? string.Empty;

			_mode = AuthorizationMode.Required;
		}
		#endregion

		#region 公共属性
		/// <summary>
		/// 获取或设置授权验证的方式。
		/// </summary>
		public AuthorizationMode Mode
		{
			get
			{
				return _mode;
			}
			set
			{
				_mode = value;
			}
		}

		/// <summary>
		/// 获取或设置操作编号。
		/// </summary>
		public string ActionId
		{
			get
			{
				return _actionId;
			}
			set
			{
				_actionId = value;
			}
		}

		/// <summary>
		/// 获取或设置模式编号。
		/// </summary>
		public string SchemaId
		{
			get
			{
				return _schemaId;
			}
			set
			{
				_schemaId = value;
			}
		}

		/// <summary>
		/// 获取凭证验证器实例。
		/// </summary>
		public ICredentialValidator Validator
		{
			get
			{
				if(_validator == null)
				{
					var type = this.ValidatorType;

					if(type == null)
						return null;

					lock(type)
					{
						if(_validator == null)
							_validator = Activator.CreateInstance(type) as ICredentialValidator;
					}
				}

				return _validator;
			}
		}

		/// <summary>
		/// 获取或设置凭证验证器的类型。
		/// </summary>
		public Type ValidatorType
		{
			get
			{
				return _validatorType;
			}
			set
			{
				if(_validatorType == value)
					return;

				if(value != null && !typeof(ICredentialValidator).IsAssignableFrom(value))
					throw new ArgumentException();

				_validatorType = value;
				_validator = null;
			}
		}
		#endregion
	}
}
