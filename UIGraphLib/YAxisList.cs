//============================================================================
//ZedGraph Class Library - A Flexible Line Graph/Bar Graph Library in C#
//Copyright ?2005  John Champion
//
//This library is free software; you can redistribute it and/or
//modify it under the terms of the GNU Lesser General Public
//License as published by the Free Software Foundation; either
//version 2.1 of the License, or (at your option) any later version.
//
//This library is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//Lesser General Public License for more details.
//
//You should have received a copy of the GNU Lesser General Public
//License along with this library; if not, write to the Free Software
//Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//=============================================================================

#region Using directives

using System;
using System.Collections.Generic;

#endregion

namespace UIGraphLib
{
	/// <summary>
	/// A collection class containing a list of <see c_ref="YAxis"/> objects.
	/// </summary>
	/// 
	/// <author>John Champion</author>
	/// <version> $Revision: 3.3 $ $Date: 2006-06-24 20:26:43 $ </version>
	[Serializable]
	public class YAxisList : List<YAxis>, ICloneable
	{

		#region Constructors

		/// <summary>
		/// Default constructor for the collection class.
		/// </summary>
		public YAxisList()
		{
		}

		/// <summary>
		/// The Copy Constructor
		/// </summary>
		/// <param name="rhs">The <see c_ref="YAxisList"/> object from which to copy</param>
		public YAxisList( YAxisList rhs )
		{
			foreach ( YAxis item in rhs )
			{
				Add( item.Clone() );
			}
		}

		/// <summary>
		/// Implement the <see c_ref="ICloneable" /> interface in a typesafe manner by just
		/// calling the typed version of <see c_ref="Clone" />
		/// </summary>
		/// <returns>A deep copy of this object</returns>
		object ICloneable.Clone()
		{
			return Clone();
		}

		/// <summary>
		/// Typesafe, deep-copy clone method.
		/// </summary>
		/// <returns>A new, independent copy of this class</returns>
		public YAxisList Clone()
		{
			return new YAxisList( this );
		}

		#endregion

		#region List Methods

		/// <summary>
		/// Indexer to access the specified <see c_ref="Axis"/> object by
		/// its ordinal position in the list.
		/// </summary>
		/// <param name="index">The ordinal position (zero-based) of the
		/// <see c_ref="YAxis"/> object to be accessed.</param>
		/// <value>An <see c_ref="Axis"/> object reference.</value>
		public new YAxis this[int index]
		{
			get { return ( ( ( index < 0 || index >= Count ) ? null : base[index] ) ); }
		}

		/// <summary>
		/// Indexer to access the specified <see c_ref="Axis"/> object by
		/// its <see c_ref="Axis.Title"/> string.
		/// </summary>
		/// <param name="title">The string title of the
		/// <see c_ref="YAxis"/> object to be accessed.</param>
		/// <value>A <see c_ref="Axis"/> object reference.</value>
		public YAxis this[string title]
		{
			get
			{
				int index = IndexOf( title );
				if ( index >= 0 )
					return this[index];
			    return null;
			}
		}

		/// <summary>
		/// Return the zero-based position index of the
		/// <see c_ref="Axis"/> with the specified <see c_ref="Axis.Title"/>.
		/// </summary>
		/// <remarks>The comparison of titles is not case sensitive, but it must include
		/// all characters including punctuation, spaces, etc.</remarks>
		/// <param name="title">The <see c_ref="String"/> label that is in the
		/// <see c_ref="Axis.Title"/> attribute of the item to be found.
		/// </param>
		/// <returns>The zero-based index of the specified <see c_ref="Axis"/>,
		/// or -1 if the <see c_ref="Axis.Title"/> was not found in the list</returns>
		/// <seealso c_ref="IndexOfTag"/>
		public int IndexOf( string title )
		{
			int index = 0;
			foreach ( YAxis axis in this )
			{
				if ( String.Compare( axis.Title._text, title, true ) == 0 )
					return index;
				index++;
			}

			return -1;
		}

		/// <summary>
		/// Return the zero-based position index of the
		/// <see c_ref="Axis"/> with the specified <see c_ref="Axis.Tag" />.
		/// </summary>
		/// <remarks>In order for this method to work, the <see c_ref="Axis.Tag" />
		/// property must be of type <see c_ref="String"/>.
		/// </remarks>
		/// <param name="tagStr">The <see c_ref="String"/> tag that is in the
		/// <see c_ref="Axis.Tag" /> attribute of the item to be found.
		/// </param>
		/// <returns>The zero-based index of the specified <see c_ref="Axis" />,
		/// or -1 if the <see c_ref="Axis.Tag" /> string is not in the list</returns>
		public int IndexOfTag( string tagStr )
		{
			int index = 0;
			foreach ( YAxis axis in this )
			{
				if ( axis.Tag is string &&
					String.Compare( (string)axis.Tag, tagStr, true ) == 0 )
					return index;
				index++;
			}

			return -1;
		}

		/// <summary>
		/// Create a new <see c_ref="YAxis" /> and add it to this list.
		/// </summary>
		/// <param name="title">The title string for the new axis</param>
		/// <returns>An integer representing the ordinal position of the new <see c_ref="YAxis" /> in
		/// this <see c_ref="YAxisList" />.  This is the value that you would set the
		/// <see c_ref="CurveItem.YAxisIndex" /> property of a given <see c_ref="CurveItem" /> to 
		/// assign it to this new <see c_ref="YAxis" />.</returns>
		public int Add( string title )
		{
			YAxis axis = new YAxis( title );
			Add( axis );

			return Count - 1;
		}

		#endregion

	}
}
