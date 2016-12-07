window.convertText = (that) ->

	$(that).html -> 
		$(that)
		.html()
		.replace /((<div.*>).*(<\/div>))/g, ""

	directiveLine = 'data-toggle="tooltip" data-placement="top"'
	
	$(that).html ->
		$(that)	
		.text()
		.replace /(([^\u0000-\u007F]|\w)+'?(([^\u0000-\u007F]|\w)+)?)+/g, (match, value) ->
			"<span #{directiveLine}>#{value}</span>"
		.replace /(\<[span]{4}(\s+)([A-Za-z0-9-\=\ \"]*)\>)(\s*)(\<\/[span]{4}\>)/gm, ""
	
	$('[data-toggle="tooltip"]')
		.mouseenter ->
			$(this).attr 'data-original-title', "Loading..."
		.tooltip()
