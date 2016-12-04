var convertText = function (that) {

	let htmlWithoutTooltip = $(that).html().replace(
		/((<div.*>).*(<\/div>))/g, function (match, value) {
			return "";
		}
	);

	$(that).html(htmlWithoutTooltip);

	let text = $(that).text();

	let directiveLine = 'data-toggle="tooltip" data-placement="top"';

	let html = text.replace(
		/(([^\u0000-\u007F]|\w)+'?(([^\u0000-\u007F]|\w)+)?)+/g, function (match, value) {
			return `<span ${directiveLine}>${value}</span>`;
		}
	);

	let pureHtml = html.replace(
		/(\<[span]{4}(\s+)([A-Za-z0-9-\=\ \"]*)\>)(\s*)(\<\/[span]{4}\>)/gm, function (match, value) {
			return "";
		}
	);

	$(that).html(pureHtml);

	$('[data-toggle="tooltip"]').mouseenter(function () {
		$(this).attr('data-original-title', "Loading...");
	});

	$(function () {
		$('[data-toggle="tooltip"]').tooltip()
	})

};

