//============================================================================
//ZedGraph Class Library - A Flexible Line Graph/Bar Graph Library in C#
//Copyright ?2004  John Champion
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

using System;
using System.Drawing;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace UIGraphLib
{
	/// <summary>
	/// This class encapsulates the chart <see c_ref="Legend"/> that is displayed
	/// in the <see c_ref="GraphPane"/>
	/// </summary>
	/// 
	/// <author> John Champion </author>
	/// <version> $Revision: 3.41 $ $Date: 2007-08-11 19:24:55 $ </version>
	[Serializable]
	public class Legend : ICloneable, ISerializable
	{
	#region private Fields

		/// <summary> Private field to hold the bounding rectangle around the legend.
		/// This bounding rectangle varies with the number of legend entries, font sizes,
		/// etc., and is re-calculated by <see c_ref="Legend.CalcRect"/> at each redraw.
		/// Use the public readonly property <see c_ref="Legend.Rect"/> to access this
		/// rectangle.
		/// </summary>
		private RectangleF _rect;
		/// <summary>Private field to hold the legend location setting.  This field
		/// contains the <see c_ref="LegendPos"/> enum type to specify the area of
		/// the graph where the legend will be positioned.  Use the public property
		/// <see c_ref="LegendPos"/> to access this value.
		/// </summary>
		/// <seealso c_ref="Default.Position"/>
		private LegendPos _position;
		/// <summary>
		/// Private field to enable/disable horizontal stacking of the legend entries.
		/// If this value is false, then the legend entries will always be a single column.
		/// Use the public property <see c_ref="IsHStack"/> to access this value.
		/// </summary>
		/// <seealso c_ref="Default.IsHStack"/>
		private bool _isHStack;
		/// <summary>
		/// Private field to enable/disable drawing of the entire legend.
		/// If this value is false, then the legend will not be drawn.
		/// Use the public property <see c_ref="IsVisible"/> to access this value.
		/// </summary>
		private bool _isVisible;
		/// <summary>
		/// Private field that stores the <see c_ref="ZedGraph.Fill"/> data for this
		/// <see c_ref="Legend"/>.  Use the public property <see c_ref="Fill"/> to
		/// access this value.
		/// </summary>
		private Fill _fill;
		/// <summary>
		/// Private field that stores the <see c_ref="ZedGraph.Border"/> data for this
		/// <see c_ref="Legend"/>.  Use the public property <see c_ref="Border"/> to
		/// access this value.
		/// </summary>
		private Border _border;
		/// <summary>
		/// Private field to maintain the <see c_ref="FontSpec"/> class that
		/// maintains font attributes for the entries in this legend.  Use
		/// the <see c_ref="FontSpec"/> property to access this class.
		/// </summary>
		private FontSpec _fontSpec;
		/// <summary>
		/// Private field to maintain the <see c_ref="Legend"/> location.  This object
		/// is only applicable if the <see c_ref="Position"/> property is set to
		/// <see c_ref="LegendPos.Float"/>.
		/// </summary>
		private Location _location;

		/// <summary>
		/// Private temporary field to maintain the number of columns (horizontal stacking) to be used
		/// for drawing the <see c_ref="Legend"/>.  This value is only valid during a draw operation.
		/// </summary>
		private int _hStack;
		/// <summary>
		/// Private temporary field to maintain the width of each column in the
		/// <see c_ref="Legend"/>.  This value is only valid during a draw operation.
		/// </summary>
		private float _legendItemWidth;
		/// <summary>
		/// Private temporary field to maintain the height of each row in the
		/// <see c_ref="Legend"/>.  This value is only valid during a draw operation.
		/// </summary>
		private float _legendItemHeight;

		/// <summary>
		/// Private field to store the gap between the legend and the chart rectangle.
		/// </summary>
		private float _gap;

		// CJBL
		/// <summary>
		/// Private field to select output order of legend entries.
		/// </summary>
		private bool _isReverse;

		/// <summary>
		/// Private temporary field to maintain the characteristic "gap" for the legend.
		/// This is normal the height of the largest font in the legend.
		/// This value is only valid during a draw operation.
		/// </summary>
		private float _tmpSize;

		/// <summary>
		/// Private field to enable/diable drawing the line and symbol samples in the
		/// legend.
		/// </summary>
		private bool _isShowLegendSymbols;

	#endregion

	#region Defaults
		/// <summary>
		/// A simple struct that defines the
		/// default property values for the <see c_ref="Legend"/> class.
		/// </summary>
		public struct Default
		{
			// Default Legend properties
			/// <summary>
			/// The default pen width for the <see c_ref="Legend"/> border border.
			/// (<see c_ref="ZedGraph.LineBase.Width"/> property).  Units are in pixels.
			/// </summary>
			public static float BorderWidth = 1;
			/// <summary>
			/// The default color for the <see c_ref="Legend"/> border border.
			/// (<see c_ref="ZedGraph.LineBase.Color"/> property). 
			/// </summary>
			public static Color BorderColor = Color.Black;
			/// <summary>
			/// The default color for the <see c_ref="Legend"/> background.
			/// (<see c_ref="ZedGraph.Fill.Color"/> property).  Use of this
			/// color depends on the status of the <see c_ref="ZedGraph.Fill.Type"/>
			/// property.
			/// </summary>
			public static Color FillColor = Color.White;
			/// <summary>
			/// The default custom brush for filling in this <see c_ref="Legend"/>.
			/// </summary>
			public static Brush FillBrush = null;
			/// <summary>
			/// The default fill mode for the <see c_ref="Legend"/> background.
			/// </summary>
			public static FillType FillType = FillType.Brush;
			/// <summary>
			/// The default location for the <see c_ref="Legend"/> on the graph
			/// (<see c_ref="Legend.Location"/> property).  This property is
			/// defined as a <see c_ref="LegendPos"/> enumeration.
			/// </summary>
			public static LegendPos Position = LegendPos.Top;
			/// <summary>
			/// The default border mode for the <see c_ref="Legend"/>.
			/// (<see c_ref="ZedGraph.LineBase.IsVisible"/> property). true
			/// to draw a border around the <see c_ref="Legend.Rect"/>,
			/// false otherwise.
			/// </summary>
			public static bool IsBorderVisible = true;
			/// <summary>
			/// The default display mode for the <see c_ref="Legend"/>.
			/// (<see c_ref="Legend.IsVisible"/> property). true
			/// to show the legend,
			/// false to hide it.
			/// </summary>
			public static bool IsVisible = true;
			/// <summary>
			/// The default fill mode for the <see c_ref="Legend"/> background
			/// (<see c_ref="ZedGraph.Fill.Type"/> property).
			/// true to fill-in the background with color,
			/// false to leave the background transparent.
			/// </summary>
			public static bool IsFilled = true;
			/// <summary>
			/// The default horizontal stacking mode for the <see c_ref="Legend"/>
			/// (<see c_ref="Legend.IsHStack"/> property).
			/// true to allow horizontal legend item stacking, false to allow
			/// only vertical legend orientation.
			/// </summary>
			public static bool IsHStack = true;

			/// <summary>
			/// The default font family for the <see c_ref="Legend"/> entries
			/// (<see c_ref="ZedGraph.FontSpec.Family"/> property).
			/// </summary>
			public static string FontFamily = "Arial";
			/// <summary>
			/// The default font size for the <see c_ref="Legend"/> entries
			/// (<see c_ref="ZedGraph.FontSpec.Size"/> property).  Units are
			/// in points (1/72 inch).
			/// </summary>
			public static float FontSize = 12;
			/// <summary>
			/// The default font color for the <see c_ref="Legend"/> entries
			/// (<see c_ref="ZedGraph.FontSpec.FontColor"/> property).
			/// </summary>
			public static Color FontColor = Color.Black;
			/// <summary>
			/// The default font bold mode for the <see c_ref="Legend"/> entries
			/// (<see c_ref="ZedGraph.FontSpec.IsBold"/> property). true
			/// for a bold typeface, false otherwise.
			/// </summary>
			public static bool FontBold = false;
			/// <summary>
			/// The default font italic mode for the <see c_ref="Legend"/> entries
			/// (<see c_ref="ZedGraph.FontSpec.IsItalic"/> property). true
			/// for an italic typeface, false otherwise.
			/// </summary>
			public static bool FontItalic = false;
			/// <summary>
			/// The default font underline mode for the <see c_ref="Legend"/> entries
			/// (<see c_ref="ZedGraph.FontSpec.IsUnderline"/> property). true
			/// for an underlined typeface, false otherwise.
			/// </summary>
			public static bool FontUnderline = false;
			/// <summary>
			/// The default color for filling in the scale text background
			/// (see <see c_ref="ZedGraph.Fill.Color"/> property).
			/// </summary>
			public static Color FontFillColor = Color.White;
			/// <summary>
			/// The default custom brush for filling in the scale text background
			/// (see <see c_ref="ZedGraph.Fill.Brush"/> property).
			/// </summary>
			public static Brush FontFillBrush = null;
			/// <summary>
			/// The default fill mode for filling in the scale text background
			/// (see <see c_ref="ZedGraph.Fill.Type"/> property).
			/// </summary>
			public static FillType FontFillType = FillType.None;

			/// <summary>
			/// The default gap size between the legend and the <see c_ref="Chart.Rect" />.
			/// This is the default value of <see c_ref="Legend.Gap" />.
			/// </summary>
			public static float Gap = 0.5f;

			/// <summary>
			/// Default value for the <see c_ref="Legend.IsReverse" /> property.
			/// </summary>
			public static bool IsReverse = false;

			/// <summary>
			/// Default value for the <see c_ref="Legend.IsShowLegendSymbols" /> property.
			/// </summary>
			public static bool IsShowLegendSymbols = true;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Get the bounding rectangle for the <see c_ref="Legend"/> in screen coordinates
		/// </summary>
		/// <value>A screen rectangle in pixel units</value>
		public RectangleF Rect
		{
			get { return _rect; }
		}
		/// <summary>
		/// Access to the <see c_ref="ZedGraph.FontSpec"/> class used to render
		/// the <see c_ref="Legend"/> entries
		/// </summary>
		/// <value>A reference to a <see c_ref="Legend"/> object</value>
		/// <seealso c_ref="Default.FontColor"/>
		/// <seealso c_ref="Default.FontBold"/>
		/// <seealso c_ref="Default.FontItalic"/>
		/// <seealso c_ref="Default.FontUnderline"/>
		/// <seealso c_ref="Default.FontFamily"/>
		/// <seealso c_ref="Default.FontSize"/>
		public FontSpec FontSpec
		{
			get { return _fontSpec; }
			set
			{
				if ( value == null )
					throw new ArgumentNullException( "Uninitialized FontSpec in Legend" );
				_fontSpec = value;
			}
		}
		/// <summary>
		/// Gets or sets a property that shows or hides the <see c_ref="Legend"/> entirely
		/// </summary>
		/// <value> true to show the <see c_ref="Legend"/>, false to hide it </value>
		/// <seealso c_ref="Default.IsVisible"/>
		public bool IsVisible
		{
			get { return _isVisible; }
			set { _isVisible = value; }
		}
		/// <summary>
		/// The <see c_ref="Border"/> class used to draw the border border around this <see c_ref="Legend"/>.
		/// </summary>
		public Border Border
		{
			get { return _border; }
			set { _border = value; }
		}
		/// <summary>
		/// Gets or sets the <see c_ref="ZedGraph.Fill"/> data for this
		/// <see c_ref="Legend"/> background.
		/// </summary>
		public Fill Fill
		{
			get { return _fill; }
			set { _fill = value; }
		}

		/// <summary>
		/// Sets or gets a property that allows the <see c_ref="Legend"/> items to
		/// stack horizontally in addition to the vertical stacking
		/// </summary>
		/// <value>true to allow horizontal stacking, false otherwise
		/// </value>
		/// <seealso c_ref="Default.IsHStack"/>
		public bool IsHStack
		{
			get { return _isHStack; }
			set { _isHStack = value; }
		}
		/// <summary>
		/// Sets or gets the location of the <see c_ref="Legend"/> on the
		/// <see c_ref="GraphPane"/> using the <see c_ref="LegendPos"/> enum type
		/// </summary>
		/// <seealso c_ref="Default.Position"/>
		public LegendPos Position
		{
			get { return _position; }
			set { _position = value; }
		}
		/// <summary>
		/// Gets or sets the <see c_ref="Location"/> data for the <see c_ref="Legend"/>.
		/// This property is only applicable if <see c_ref="Position"/> is set
		/// to <see c_ref="LegendPos.Float"/>.
		/// </summary>
		public Location Location
		{
			get { return _location; }
			set { _location = value; }
		}

		/// <summary>
		/// Gets or sets the gap size between the legend and the <see c_ref="Chart.Rect" />.
		/// </summary>
		/// <remarks>
		/// This is expressed as a fraction of the largest scaled character height for any
		/// of the fonts used in the legend.  Each <see c_ref="CurveItem" /> in the legend can
		/// optionally have its own <see c_ref="FontSpec" /> specification.
		/// </remarks>
		public float Gap
		{
			get { return _gap; }
			set { _gap = value; }
		}

		/// <summary>
		/// Gets or sets a value that determines if the legend entries are displayed in normal order
		/// (matching the order in the <see c_ref="CurveList" />, or in reverse order.
		/// </summary>
		public bool IsReverse
		{
			get { return _isReverse; }
			set { _isReverse = value; }
		}

		/// <summary>
		/// Gets or sets a value that determines whether the line and symbol keys will be displayed
		/// in the legend.
		/// </summary>
		/// <remarks>
		/// Note: If this value is set to false (so that only the curve label text is displayed
		/// with no legend keys), then the color of the font for the legend entry of each curve
		/// will automatically be set to match the <see c_ref="CurveItem.Color"/> setting for that curve.
		/// You can override this behavior by specifying a specific font to be used for each
		/// individual curve with the <see c_ref="ZedGraph.Label.FontSpec">CurveItem.Label.FontSpec</see>
		/// property.
		/// </remarks>
		public bool IsShowLegendSymbols
		{
			get { return _isShowLegendSymbols; }
			set { _isShowLegendSymbols = value; }
		}

		#endregion

		#region Constructors
		/// <summary>
		/// Default constructor that sets all <see c_ref="Legend"/> properties to default
		/// values as defined in the <see c_ref="Default"/> class.
		/// </summary>
		public Legend()
		{
			_position = Default.Position;
			_isHStack = Default.IsHStack;
			_isVisible = Default.IsVisible;
			Location = new Location( 0, 0, CoordType.PaneFraction );

			_fontSpec = new FontSpec( Default.FontFamily, Default.FontSize,
				Default.FontColor, Default.FontBold,
				Default.FontItalic, Default.FontUnderline,
				Default.FontFillColor, Default.FontFillBrush,
				Default.FontFillType );
			_fontSpec.Border.IsVisible = false;

			_border = new Border( Default.IsBorderVisible, Default.BorderColor, Default.BorderWidth );
			_fill = new Fill( Default.FillColor, Default.FillBrush, Default.FillType );

			_gap = Default.Gap;

			_isReverse = Default.IsReverse;

			_isShowLegendSymbols = Default.IsShowLegendSymbols;
		}

		/// <summary>
		/// The Copy Constructor
		/// </summary>
		/// <param name="rhs">The XAxis object from which to copy</param>
		public Legend( Legend rhs )
		{
			_rect = rhs.Rect;
			_position = rhs.Position;
			_isHStack = rhs.IsHStack;
			_isVisible = rhs.IsVisible;

			_location = rhs.Location;
			_border = rhs.Border.Clone();
			_fill = rhs.Fill.Clone();

			_fontSpec = rhs.FontSpec.Clone();

			_gap = rhs._gap;

			_isReverse = rhs._isReverse;

			_isShowLegendSymbols = rhs._isShowLegendSymbols;
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
		public Legend Clone()
		{
			return new Legend( this );
		}

		#endregion

		#region Serialization
		/// <summary>
		/// Current schema value that defines the version of the serialized file
		/// </summary>
		public const int schema = 12;

		/// <summary>
		/// Constructor for deserializing objects
		/// </summary>
		/// <param name="info">A <see c_ref="SerializationInfo"/> instance that defines the serialized data
		/// </param>
		/// <param name="context">A <see c_ref="StreamingContext"/> instance that contains the serialized data
		/// </param>
		protected Legend( SerializationInfo info, StreamingContext context )
		{
			// The schema value is just a file version parameter.  You can use it to make future versions
			// backwards compatible as new member variables are added to classes
			int sch = info.GetInt32( "schema" );

			_position = (LegendPos)info.GetValue( "position", typeof( LegendPos ) );
			_isHStack = info.GetBoolean( "isHStack" );
			_isVisible = info.GetBoolean( "isVisible" );
			_fill = (Fill)info.GetValue( "fill", typeof( Fill ) );
			_border = (Border)info.GetValue( "border", typeof( Border ) );
			_fontSpec = (FontSpec)info.GetValue( "fontSpec", typeof( FontSpec ) );
			_location = (Location)info.GetValue( "location", typeof( Location ) );

			_gap = info.GetSingle( "gap" );

			if ( schema >= 11 )
				_isReverse = info.GetBoolean( "isReverse" );

			if ( schema >= 12 )
				_isShowLegendSymbols = info.GetBoolean( "isShowLegendSymbols" );
		}
		/// <summary>
		/// Populates a <see c_ref="SerializationInfo"/> instance with the data needed to serialize the target object
		/// </summary>
		/// <param name="info">A <see c_ref="SerializationInfo"/> instance that defines the serialized data</param>
		/// <param name="context">A <see c_ref="StreamingContext"/> instance that contains the serialized data</param>
		[SecurityPermission( SecurityAction.Demand, SerializationFormatter = true )]
		public virtual void GetObjectData( SerializationInfo info, StreamingContext context )
		{
			info.AddValue( "schema", schema );
			info.AddValue( "position", _position );
			info.AddValue( "isHStack", _isHStack );
			info.AddValue( "isVisible", _isVisible );
			info.AddValue( "fill", _fill );
			info.AddValue( "border", _border );
			info.AddValue( "fontSpec", _fontSpec );
			info.AddValue( "location", _location );

			info.AddValue( "gap", _gap );
			info.AddValue( "isReverse", _isReverse );
			info.AddValue( "isShowLegendSymbols", _isShowLegendSymbols );
		}
		#endregion

		#region Rendering Methods

		/// <summary>
		/// Render the <see c_ref="Legend"/> to the specified <see c_ref="Graphics"/> device.
		/// </summary>
		/// <remarks>
		/// This method is normally only called by the Draw method
		/// of the parent <see c_ref="GraphPane"/> object.
		/// </remarks>
		/// <param name="g">
		/// A graphic device object to be drawn into.  This is normally e.Graphics from the
		/// PaintEventArgs argument to the Paint() method.
		/// </param>
		/// <param name="pane">
		/// A reference to the <see c_ref="PaneBase"/> object that is the parent or
		/// owner of this object.
		/// </param>
		/// <param name="scaleFactor">
		/// The scaling factor to be used for rendering objects.  This is calculated and
		/// passed down by the parent <see c_ref="GraphPane"/> object using the
		/// <see c_ref="PaneBase.CalcScaleFactor"/> method, and is used to proportionally adjust
		/// font sizes, etc. according to the actual size of the graph.
		/// </param>
		public void Draw( Graphics g, PaneBase pane, float scaleFactor )
		{
			// if the legend is not visible, do nothing
			if ( !_isVisible )
				return;

			// Fill the background with the specified color if required
			_fill.Draw( g, _rect );

			PaneList paneList = GetPaneList( pane );

			float halfGap = _tmpSize / 2.0F;

			// Check for bad data values
			if ( _hStack <= 0 )
				_hStack = 1;
			if ( _legendItemWidth <= 0 )
				_legendItemWidth = 100;
			if ( _legendItemHeight <= 0 )
				_legendItemHeight = _tmpSize;

			//float gap = pane.ScaledGap( scaleFactor );

			int iEntry = 0;
			float x, y;

			// Get a brush for the legend label text
			using ( SolidBrush brushB = new SolidBrush( Color.Black ) )
			{
				foreach ( GraphPane tmpPane in paneList )
				{
					// Loop for each curve in the CurveList collection
					//foreach ( CurveItem curve in tmpPane.CurveList )
					int count = tmpPane.CurveList.Count;
					for ( int i = 0; i < count; i++ )
					{
						CurveItem curve = tmpPane.CurveList[_isReverse ? count - i - 1 : i];

						if ( curve._label._text != "" && curve._label._isVisible )
						{
							// Calculate the x,y (TopLeft) location of the current
							// curve legend label
							// assuming:
							//  charHeight/2 for the left margin, plus legendWidth for each
							//    horizontal column
							//  legendHeight is the line spacing, with no extra margin above

							x = _rect.Left + halfGap / 2.0F +
								( iEntry % _hStack ) * _legendItemWidth;
							y = _rect.Top + iEntry / _hStack * _legendItemHeight;

							// Draw the legend label for the current curve
							FontSpec tmpFont = ( curve._label._fontSpec != null ) ?
										curve._label._fontSpec : FontSpec;

							// This is required because, for long labels, the centering can affect the
							// position in GDI+.
							tmpFont.StringAlignment = StringAlignment.Near;

							if ( _isShowLegendSymbols )
							{
								tmpFont.Draw( g, pane, curve._label._text,
										x + 2.5F * _tmpSize, y + _legendItemHeight / 2.0F+1,
										AlignH.Left, AlignV.Center, scaleFactor );

								RectangleF rect = new RectangleF( x, y + _legendItemHeight / 4.0F,
									2 * _tmpSize, _legendItemHeight / 2.0F );
								curve.DrawLegendKey( g, tmpPane, rect, scaleFactor );
							}
							else
							{
								if ( curve._label._fontSpec == null )
									tmpFont.FontColor = curve.Color;

								tmpFont.Draw(g, pane, curve._label._text,
									x + 0.0F * _tmpSize, y + _legendItemHeight / 2.0F,
									AlignH.Left, AlignV.Center, scaleFactor);
							}

							// maintain a curve count for positioning
							iEntry++;
						}
					}
					if ( pane is MasterPane && ( (MasterPane)pane ).IsUniformLegendEntries )
						break;
				}

				// Draw a border around the legend if required
				if ( iEntry > 0 )
					Border.Draw( g, pane, scaleFactor, _rect );
			}
		}

		private float GetMaxHeight( PaneList paneList, Graphics g, float scaleFactor )
		{
			// Set up some scaled dimensions for calculating sizes and locations
			float defaultCharHeight = FontSpec.GetHeight( scaleFactor );
			float maxCharHeight = defaultCharHeight;

			// Find the largest charHeight, just in case the curves have individual fonts defined
			foreach ( GraphPane tmpPane in paneList )
			{
				foreach ( CurveItem curve in tmpPane.CurveList )
				{
					if ( curve._label._text != string.Empty && curve._label._isVisible )
					{
						float tmpHeight = defaultCharHeight;
						if ( curve._label._fontSpec != null )
							tmpHeight = curve._label._fontSpec.GetHeight( scaleFactor );

						// Account for multiline legend entries
						tmpHeight *= curve._label._text.Split( '\n' ).Length;

						if ( tmpHeight > maxCharHeight )
							maxCharHeight = tmpHeight;
					}
				}
			}

			return maxCharHeight;
		}

		/// <summary>
		/// Determine if a mouse point is within the legend, and if so, which legend
		/// entry (<see c_ref="CurveItem"/>) is nearest.
		/// </summary>
		/// <param name="mousePt">The screen point, in pixel coordinates.</param>
		/// <param name="pane">
		/// A reference to the <see c_ref="PaneBase"/> object that is the parent or
		/// owner of this object.
		/// </param>
		/// <param name="scaleFactor">
		/// The scaling factor to be used for rendering objects.  This is calculated and
		/// passed down by the parent <see c_ref="GraphPane"/> object using the
		/// <see c_ref="PaneBase.CalcScaleFactor"/> method, and is used to proportionally adjust
		/// font sizes, etc. according to the actual size of the graph.
		/// </param>
		/// <param name="index">The index number of the <see c_ref="CurveItem"/> legend
		/// entry that is under the mouse point.  The <see c_ref="CurveItem"/> object is
		/// accessible via <see c_ref="GraphPane.CurveList">CurveList[index]</see>.
		/// </param>
		/// <returns>true if the mouse point is within the <see c_ref="Legend"/> bounding
		/// box, false otherwise.</returns>
		/// <seealso c_ref="GraphPane.FindNearestObject"/>
		public bool FindPoint( PointF mousePt, PaneBase pane, float scaleFactor, out int index )
		{
			index = -1;

			if ( _rect.Contains( mousePt ) )
			{
				int j = (int)( ( mousePt.Y - _rect.Top ) / _legendItemHeight );
				int i = (int)( ( mousePt.X - _rect.Left - _tmpSize / 2.0f ) / _legendItemWidth );
				if ( i < 0 )
					i = 0;
				if ( i >= _hStack )
					i = _hStack - 1;

				int pos = i + j * _hStack;
				index = 0;

				PaneList paneList = GetPaneList( pane );

				foreach ( GraphPane tmpPane in paneList )
				{
					foreach ( CurveItem curve in tmpPane.CurveList )
					{
						if ( curve._label._isVisible && curve._label._text != string.Empty )
						{
							if ( pos == 0 )
								return true;
							pos--;
						}
						index++;
					}
				}

				return true;
			}
		    return false;
		}

		private PaneList GetPaneList( PaneBase pane )
		{
			// For a single GraphPane, create a PaneList to contain it
			// Otherwise, just use the paneList from the MasterPane
			PaneList paneList;

			if ( pane is GraphPane )
			{
				paneList = new PaneList();
				paneList.Add( (GraphPane)pane );
			}
			else
				paneList = ( (MasterPane)pane ).PaneList;

			return paneList;
		}

		/// <summary>
		/// Calculate the <see c_ref="Legend"/> rectangle (<see c_ref="Rect"/>),
		/// taking into account the number of required legend
		/// entries, and the legend drawing preferences.
		/// </summary>
		/// <remarks>Adjust the size of the
		/// <see c_ref="Chart.Rect"/> for the parent <see c_ref="GraphPane"/> to accomodate the
		/// space required by the legend.
		/// </remarks>
		/// <param name="g">
		/// A graphic device object to be drawn into.  This is normally e.Graphics from the
		/// PaintEventArgs argument to the Paint() method.
		/// </param>
		/// <param name="pane">
		/// A reference to the <see c_ref="PaneBase"/> object that is the parent or
		/// owner of this object.
		/// </param>
		/// <param name="scaleFactor">
		/// The scaling factor to be used for rendering objects.  This is calculated and
		/// passed down by the parent <see c_ref="GraphPane"/> object using the
		/// <see c_ref="PaneBase.CalcScaleFactor"/> method, and is used to proportionally adjust
		/// font sizes, etc. according to the actual size of the graph.
		/// </param>
		/// <param name="tChartRect">
		/// The rectangle that contains the area bounded by the axes, in pixel units.
		/// <seealso c_ref="Chart.Rect" />
		/// </param>
		public void CalcRect( Graphics g, PaneBase pane, float scaleFactor,
			ref RectangleF tChartRect )
		{
			// Start with an empty rectangle
			_rect = Rectangle.Empty;
			_hStack = 1;
			_legendItemWidth = 1;
			_legendItemHeight = 0;

			RectangleF clientRect = pane.CalcClientRect( g, scaleFactor );

			// If the legend is invisible, don't do anything
			if ( !_isVisible )
				return;

			int nCurve = 0;

			PaneList paneList = GetPaneList( pane );
			_tmpSize = GetMaxHeight( paneList, g, scaleFactor );

			float halfGap = _tmpSize / 2.0F,
					maxWidth = 0,
					tmpWidth,
					gapPix = _gap * _tmpSize;

			foreach ( GraphPane tmpPane in paneList )
			{
				// Loop through each curve in the curve list
				// Find the maximum width of the legend labels
				//foreach ( CurveItem curve in tmpPane.CurveList )
				//foreach ( CurveItem curve in GetIterator( tmpPane.CurveList, _isReverse ) )
				int count = tmpPane.CurveList.Count;
				for ( int i = 0; i < count; i++ )
				{
					CurveItem curve = tmpPane.CurveList[_isReverse ? count - i - 1 : i];
					if ( curve._label._text != string.Empty && curve._label._isVisible )
					{
						// Calculate the width of the label save the max width
						FontSpec tmpFont = ( curve._label._fontSpec != null ) ?
										curve._label._fontSpec : FontSpec;

						tmpWidth = tmpFont.GetWidth( g, curve._label._text, scaleFactor );

						if ( tmpWidth > maxWidth )
							maxWidth = tmpWidth;

						// Save the maximum symbol height for line-type curves
						if ( curve is LineItem && ( (LineItem)curve ).Symbol.Size > _legendItemHeight )
							_legendItemHeight = ( (LineItem)curve ).Symbol.Size;

						nCurve++;
					}
				}

				if ( pane is MasterPane && ( (MasterPane)pane ).IsUniformLegendEntries )
					break;
			}

			float widthAvail;

			// Is this legend horizontally stacked?

			if ( _isHStack )
			{
				// Determine the available space for horizontal stacking
				switch ( _position )
				{
					// Never stack if the legend is to the right or left
					case LegendPos.Right:
					case LegendPos.Left:
						widthAvail = 0;
						break;

					// for the top & bottom, the axis border width is available
					case LegendPos.Top:
					case LegendPos.TopCenter:
					case LegendPos.Bottom:
					case LegendPos.BottomCenter:
						widthAvail = tChartRect.Width;
						break;

					// for the top & bottom flush left, the panerect less margins is available
					case LegendPos.TopFlushLeft:
					case LegendPos.BottomFlushLeft:
						widthAvail = clientRect.Width;
						break;

					// for inside the axis area or Float, use 1/2 of the axis border width
					case LegendPos.InsideTopRight:
					case LegendPos.InsideTopLeft:
					case LegendPos.InsideBotRight:
					case LegendPos.InsideBotLeft:
					case LegendPos.Float:
						widthAvail = tChartRect.Width / 2;
						break;

					// shouldn't ever happen
					default:
						widthAvail = 0;
						break;
				}

				// width of one legend entry
				if ( _isShowLegendSymbols )
					_legendItemWidth = 3.0f * _tmpSize + maxWidth;
				else
					_legendItemWidth = 0.5f * _tmpSize + maxWidth;

				// Calculate the number of columns in the legend
				// Normally, the legend is:
				//     available width / ( max width of any entry + space for line&symbol )
				if ( maxWidth > 0 )
					_hStack = (int)( ( widthAvail - halfGap ) / _legendItemWidth );

				// You can never have more columns than legend entries
				if ( _hStack > nCurve )
					_hStack = nCurve;

				// a saftey check
				if ( _hStack == 0 )
					_hStack = 1;
			}
			else
			{
				if ( _isShowLegendSymbols )
					_legendItemWidth = 3.0F * _tmpSize + maxWidth;
				else
					_legendItemWidth = 0.5F * _tmpSize + maxWidth;
			}

			// legend is:
			//   item:     space  line  space  text   space
			//   width:     wid  4*wid   wid  maxWid   wid 
			// The symbol is centered on the line
			//
			// legend begins 3 * wid to the right of the plot rect
			//
			// The height of the legend is the actual height of the lines of text
			//   (nCurve * hite) plus wid on top and wid on the bottom

			// total legend width
			float totLegWidth = _hStack * _legendItemWidth;

			// The total legend height
			_legendItemHeight = _legendItemHeight * scaleFactor + halfGap;
			if ( _tmpSize > _legendItemHeight )
				_legendItemHeight = _tmpSize;
			float totLegHeight = (float)Math.Ceiling( nCurve / (double)_hStack )
				* _legendItemHeight;

			RectangleF newRect = new RectangleF();

			// Now calculate the legend rect based on the above determined parameters
			// Also, adjust the ChartRect to reflect the space for the legend
			if ( nCurve > 0 )
			{
				newRect = new RectangleF( 0, 0, totLegWidth, totLegHeight );

				// The switch statement assigns the left and top edges, and adjusts the ChartRect
				// as required.  The right and bottom edges are calculated at the bottom of the switch.
				switch ( _position )
				{
					case LegendPos.Right:
						newRect.X = clientRect.Right - totLegWidth;
						newRect.Y = tChartRect.Top;

						tChartRect.Width -= totLegWidth + gapPix;
						break;
					case LegendPos.Top:
						newRect.X = tChartRect.Left;
						newRect.Y = clientRect.Top;

						tChartRect.Y += totLegHeight + gapPix;
						tChartRect.Height -= totLegHeight + gapPix;
						break;
					case LegendPos.TopFlushLeft:
						newRect.X = clientRect.Left;
						newRect.Y = clientRect.Top;

						tChartRect.Y += totLegHeight + gapPix * 1.5f;
						tChartRect.Height -= totLegHeight + gapPix * 1.5f;
						break;
					case LegendPos.TopCenter:
						newRect.X = tChartRect.Left + ( tChartRect.Width - totLegWidth ) / 2;
						newRect.Y = tChartRect.Top;

						tChartRect.Y += totLegHeight + gapPix;
						tChartRect.Height -= totLegHeight + gapPix;
						break;
					case LegendPos.Bottom:
						newRect.X = tChartRect.Left;
						newRect.Y = clientRect.Bottom - totLegHeight;

						tChartRect.Height -= totLegHeight + gapPix;
						break;
					case LegendPos.BottomFlushLeft:
						newRect.X = clientRect.Left;
						newRect.Y = clientRect.Bottom - totLegHeight;

						tChartRect.Height -= totLegHeight + gapPix;
						break;
					case LegendPos.BottomCenter:
						newRect.X = tChartRect.Left + ( tChartRect.Width - totLegWidth ) / 2;
						newRect.Y = clientRect.Bottom - totLegHeight;

						tChartRect.Height -= totLegHeight + gapPix;
						break;
					case LegendPos.Left:
						newRect.X = clientRect.Left;
						newRect.Y = tChartRect.Top;

						tChartRect.X += totLegWidth + halfGap;
						tChartRect.Width -= totLegWidth + gapPix;
						break;
					case LegendPos.InsideTopRight:
						newRect.X = tChartRect.Right - totLegWidth;
						newRect.Y = tChartRect.Top;
						break;
					case LegendPos.InsideTopLeft:
						newRect.X = tChartRect.Left;
						newRect.Y = tChartRect.Top;
						break;
					case LegendPos.InsideBotRight:
						newRect.X = tChartRect.Right - totLegWidth;
						newRect.Y = tChartRect.Bottom - totLegHeight;
						break;
					case LegendPos.InsideBotLeft:
						newRect.X = tChartRect.Left;
						newRect.Y = tChartRect.Bottom - totLegHeight;
						break;
					case LegendPos.Float:
						newRect.Location = Location.TransformTopLeft( pane, totLegWidth, totLegHeight );
						break;
				}
			}

			_rect = newRect;
		}

		//		/// <summary>
		//		/// Private method to the render region that gives the iterator depending on the attribute
		//		/// </summary>
		//		/// <param name="c"></param>
		//		/// <param name="forward"></param>
		//		/// <returns></returns>
		//		private IEnumerable<CurveItem> GetIterator(CurveList c, bool forward)
		//		{
		//			return forward ? c.Forward : c.Backward;
		//		}

		#endregion
	}
}

