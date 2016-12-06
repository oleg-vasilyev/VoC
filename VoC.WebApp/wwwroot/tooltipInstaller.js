(function() {
  window.installTooltip = function() {
    var entered, mouseEnter, mouseLeave, target;
    target = $('[data-toggle="tooltip"]');
    mouseEnter = Rx.Observable.fromEvent(target, 'mouseenter');
    mouseLeave = Rx.Observable.fromEvent(target, 'mouseleave');
    entered = mouseEnter.flatMap(function(current) {
      return Rx.Observable.of(current).delay(1500).takeUntil(mouseLeave);
    });
    return entered.subscribe(function(current) {
      return $.ajax({
        url: '/api/Main/GetTranslation',
        data: {
          word: current.currentTarget.innerText
        },
        headers: {
          'Authorization': 'Bearer ' + localStorage.getItem("token"),
          'Content-Type': 'application/json'
        },
        success: function(output) {
          var language, languageFromShortLanguageName, ref, reliability, reliabilityLine, shortLanguageName, tooltip;
          ref = output.split(":"), shortLanguageName = ref[0], reliability = ref[1];
          if (shortLanguageName && reliability) {
            shortLanguageName = shortLanguageName.replace(/\s/g, '');
            reliability = reliability.replace(/\s/g, '');
            languageFromShortLanguageName = {
              'en': 'English',
              'es': 'Spanish',
              'pt': 'Portuguese',
              'ru': 'Russian',
              'bg': 'Bulgarian'
            };
            reliabilityLine = ((+reliability).toFixed(3)) + "%";
            language = languageFromShortLanguageName[shortLanguageName];
            if (language) {
              tooltip = language + " " + reliabilityLine;
            }
          }
          if (!tooltip) {
            tooltip = "Unknown";
          }
          $(current.currentTarget).attr('data-original-title', tooltip);
          return $(current.currentTarget).tooltip('show');
        }
      });
    });
  };

}).call(this);
