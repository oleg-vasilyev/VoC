authorizeProcess = ->
	if not localStorage.getItem "token"
		return undefined 
	$(".voc_info-panel").removeClass "panel-danger"
	$(".voc_info-panel").addClass "panel-primary" 
	$(".voc_info-panel-content").text "Enter your text down below, than hover on the word and wait to determine the language."
	$("#registerContainer").css "display", "none"
	$(".voc_input").attr "contenteditable", "true"
	$("#loginForm").css "display", "none"
	$("#accountForm").css "display", "block"
	userName = localStorage.getItem "userName"
	$("#greeting").text "Hello #{userName}!"
	console.log "user #{userName} is authorized"

authorizeProcess()

do ->
	$(".voc_input").on "change keyup paste", ->
		convertText this
		installTooltip()
		cursorManager.setEndOfContenteditable $(this)[0]

removeError = (groupId) ->
	$("##{groupId} .help-block").text ""
	$("##{groupId}").removeClass "has-error"

addError = (groupId, error) ->
	$("##{groupId} .help-block").text error
	$("##{groupId}").addClass "has-error"

removeAllErrors = ->	
	for group in [
								"passwordRegConfirmGroup"
								"passwordRegGroup"
								"loginRegGroup"
								"loginInterGroup"
								"passwordInterGroup"
								]
		removeError group

window.onSignUpClick = ->
	$.ajax
		type: 'POST'
		url: '/api/Account/Register'
		contentType: 'application/json; charset=utf-8'
		data:
			JSON.stringify
				Email: $("#loginReg").val()
				Password: $("#passwordReg").val()
				ConfirmPassword: $("#passwordRegConfirm").val()
		success: ->
			removeAllErrors()
			userName = $("#loginReg").val()
			password = $("#passwordReg").val()
			console	.log "user #{userName} successfully added"
			login userName, password
		error: (data) ->
			if data.responseJSON.ModelState["model.ConfirmPassword"]
				addError "passwordRegConfirmGroup", "password and confirmation password do not match"
			if data.responseJSON.ModelState["model.Password"]
				addError "passwordRegGroup", "password must be at least 6 characters long"
			if data.responseJSON.ModelState[""]
				if data.responseJSON.ModelState[""]["0"] 
					addError "loginRegGroup", "email is invalid"
				if data.responseJSON.ModelState[""]["1"]
					addError "loginRegGroup", "email is already taken"
			if data.responseJSON.ModelState["model.Email"]
				addError "loginRegGroup", "email field is required"

window.onLoginClick = -> 
	login $("#loginInter").val(), $("#passwordInter").val()

window.onLogoutClick = ->
	$.ajax
		type: 'POST'
		url: 'api/Account/Logout'
		contentType: 'application/json; charset=utf-8'
		headers:
			'Authorization': 'Bearer ' + localStorage.getItem("token")
			'Content-Type': 'application/json'
		success: ->
			localStorage.removeItem "token"
			localStorage.removeItem "userName"
			$(".voc_info-panel").removeClass "panel-primary"
			$(".voc_info-panel").addClass "panel-danger"
			$(".voc_info-panel-content").text "Hello! Please, sign in."
			$(".voc_input").empty()
			$(".voc_input").attr "contenteditable", "false"

			$("#loginForm").css "display", "block"
			$("#accountForm").css "display", "none"

			$("#registerContainer").css "display", "block" 
			$('#topList tr').detach()
			console.log "logout is done"

login = (username, password) ->
	$.ajax
		type: 'POST'
		url: '/Token'
		data:
			grant_type: 'password'
			username: username
			password: password
		success: (data) ->
			localStorage.setItem "token", data.access_token
			localStorage.setItem "userName", data.userName
			authorizeProcess()
			removeAllErrors()
			updateTopList()
		error: ->
			addError "loginInterGroup", "user name or password is incorrect"
			addError "passwordInterGroup", "user name or password is incorrect"

updateTopList = ->
	if not localStorage.getItem "token"
			return undefined 
	$.ajax
		url: '/api/main/gettop'
		headers:
			'Authorization': 'Bearer ' + localStorage.getItem("token")
			'Content-Type': 'application/json'
		success: (result) ->
			data = JSON.parse result
			$('#topList tr').detach()
			$('#topList').append ->
				$.map data, (ignore, index) ->
					date = new Date((data[index].LastRequest || "").replace(/-/g, "/").replace(/[TZ]/g, " "))
					return '<tr><td>' + (+index + 1) + '</td><td>' + data[index].Username + '</td><td>' + data[index].RequestCounter + '</td><td>' + date.toLocaleDateString() + " " + date.toLocaleTimeString() + '</td><td>' + data[index].AverageTime.split(".")[0] + '</td></tr>'
				.join()

$ ->
	$.connection.userTop.client.UpdateList = -> updateTopList()
	$.connection.hub.start()