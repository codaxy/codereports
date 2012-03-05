CodeReports
===========

Code reports is a small, but powerful C# reporting library.
Reports are simple to define using C# classes and attributes and can be output to text, HTML, Excel or PDF.

	[GroupingLevel(0, ShowCaption=true, CaptionFormat="{Country}", ShowHeader=true)]		
	class ReportItem
	{
		[GroupBy(0, 0, SortDirection = SortDirection.Ascending)]			
		public String Country { get; set; }

		[TableColumn(SortIndex = 0, SortDirection = SortDirection.Descending, CellAlignment=CellAlignment.Left)]
		public int Year { get; set; }

		[TableColumn(Format = "{0:0,0}")]
		public decimal? GDP { get; set; }

		[TableColumn(Format = "{0:n}", HeaderText = "GNI Per Capita")]
		public decimal? GniPerCapita { get; set; }

		[TableColumn(HeaderText="Growth", Format="{0:n}%")]
		public decimal? GdpGrowth { get; set; }
	}
	
Too see CodeReports in action check the [Pecunia demo](http://dextop.codaxy.com/pecunia) (section GDP).