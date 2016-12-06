window.installTooltip = ->
	target = $('[data-toggle="tooltip"]')

	mouseEnter = Rx.Observable.fromEvent(target, 'mouseenter')
	mouseLeave = Rx.Observable.fromEvent(target, 'mouseleave')

	entered = mouseEnter.flatMap (current) -> 
						Rx.Observable
						.of(current)
						.delay(1500)
						.takeUntil(mouseLeave)

	entered.subscribe (current) -> 
		$.ajax
			url: '/api/Main/GetTranslation'
			data:
				word : current.currentTarget.innerText
			headers: 
				'Authorization': 'Bearer ' + localStorage.getItem("token")
				'Content-Type': 'application/json'
			success: (output) -> 
				[shortLanguageName, reliability] = output.split(":")
				if shortLanguageName and reliability 
					shortLanguageName = shortLanguageName.replace(/\s/g, '')
					reliability = reliability.replace(/\s/g, '')
					languageFromShortLanguageName =
						'en' : 'English'
						'es' : 'Spanish'
						'pt' : 'Portuguese'
						'ru' : 'Russian'
						'bg' : 'Bulgarian'
					reliabilityLine = "#{(+reliability).toFixed(3)}%"
					language = languageFromShortLanguageName[shortLanguageName]
					
					if language
						tooltip = "#{language} #{reliabilityLine}"
						
				if not tooltip 
					tooltip = "Unknown"

				$(current.currentTarget).attr('data-original-title', tooltip)
				$(current.currentTarget).tooltip('show') 