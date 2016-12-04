var installTooltip = function () {

	let target = $('[data-toggle="tooltip"]');

	let mouseEnter = Rx.Observable.fromEvent(target, 'mouseenter');
	let mouseLeave = Rx.Observable.fromEvent(target, 'mouseleave');

	let entered = mouseEnter
		.flatMap(function (current) {
			return Rx.Observable
				.of(current)
				.delay(1500)
				.takeUntil(mouseLeave);
		});

	entered.subscribe(
		function (current) {

			let word = current.currentTarget.innerText;
			data = { word: word }
			$.ajax({
				url: '/api/Main/GetTranslation',
				data: data,
				headers: {
					'Authorization': 'Bearer ' + localStorage.getItem("token"),
					'Content-Type': 'application/json'
				},
			}).done(function (data) {
				let [shortLanguageName, reliability] = data.split(":");
				let tooltip;
				if (!shortLanguageName || !reliability) {
					tooltip = "Unknown";
				}
				else {
					shortLanguageName = shortLanguageName.replace(/\s/g, '');
					reliability = reliability.replace(/\s/g, '');

					let languageFromShortLanguageName = [];

					languageFromShortLanguageName['en'] = "English";
					languageFromShortLanguageName['es'] = "Spanish";
					languageFromShortLanguageName['pt'] = "Portuguese";
					languageFromShortLanguageName['ru'] = "Russian";
					languageFromShortLanguageName['bg'] = "Bulgarian";

					let reliabilityLine = (+reliability).toFixed(3) + "%";
					let language = languageFromShortLanguageName[shortLanguageName];
					if (!language) {
						language = "Unknown";
						reliabilityLine = ""
					}
					tooltip = `${language} ${reliabilityLine}`;
				}


				$(current.currentTarget).attr('data-original-title', tooltip);

				$(function () {
					$(current.currentTarget).tooltip('show')
				})
			});

		}
	)
};