// У нас было 400 строк кода, CoffeeScript, препроцессор Stylus, Gulp, собирающий фронтенд, JQuery, RxJS и целое множество правил всех сортов и расцветок, а также медиавыражения, псевдоклассы, миксины и вложенные селекторы. Не то что бы это был необходимый запас для сайта. Но если начал собирать фронтенд, становится трудно остановиться.

var gulp = require('gulp');
var stylus = require('gulp-stylus');
var watch = require('gulp-watch');
var autoprefixer = require('autoprefixer-stylus');
var cssmin = require('gulp-cssmin')

var coffee = require('gulp-coffee');

gulp.task('dev-css', function () {
	return gulp.src('styles.styl')
		.pipe(stylus())
		.pipe(gulp.dest('./'));
});

gulp.task('rel-css', function () {
	return gulp.src('styles.styl')
		.pipe(stylus({
			'include css': true,
			use: [autoprefixer('iOS >= 7', 'last 1 Chrome version')],
			compress: true
		}))
		.pipe(cssmin())
		.pipe(gulp.dest('./'))
});

gulp.task('dev-js', function() {
  gulp.src('*.coffee')
    .pipe(coffee())
    .pipe(gulp.dest('./'));
});

gulp.task('watch', function () {
	gulp.watch('*.styl', ['dev-css']);
	gulp.watch('*.coffee', ['dev-js']);
});

gulp.task('default', ['watch']);
 