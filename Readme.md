# How to create a custom View allowing collapsing of band headers


<p>This example demonstrates how to create a custom BandedGridView with custom BandedColumns. The columns now have the DefaultBandColumn property and the View draws buttons in band headers. On pressing this button columns in the band whose DefaultBandColumn option is disabled become hidden if they are visible and vise versa. Columns with DefaultBandColumn option enabled remain untouched. Such behavior is achieved by processing the CustomDrawBandHeader event in combination with MouseMove, MouseDown and MouseUp events in a BandedGridView descendant. Also we need to create a BandedColumn descendant in order to have the DefaultBandColumn property.</p>

<br/>


