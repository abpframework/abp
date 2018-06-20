var gulp = require('gulp');
var del = require('del');
var template = require('gulp-template');
var minifyCss = require('gulp-minify-css');
var rename = require('gulp-rename');
var uglify = require('gulp-uglify');
var jshint = require('gulp-jshint');
var mochaPhantomJS = require('gulp-mocha-phantomjs');
var pkg = require('./package.json');

gulp.task('clean', function () {
  return del(['dist/*']);
});

gulp.task('build-css', ['clean'], function() {
  return gulp.src('bootstrap-toc.css')
    .pipe(template(pkg))
    .pipe(gulp.dest('dist'))
    .pipe(minifyCss({compatibility: 'ie8'}))
    .pipe(rename({
      extname: '.min.css'
    }))
    .pipe(gulp.dest('dist'));
});

gulp.task('build-js', ['clean'], function() {
  return gulp.src('bootstrap-toc.js')
    .pipe(template(pkg))
    .pipe(gulp.dest('dist'))
    .pipe(uglify({
      preserveComments: 'license'
    }))
    .pipe(rename({
      extname: '.min.js'
    }))
    .pipe(gulp.dest('dist'));
});

gulp.task('js-lint', function () {
  return gulp.src('bootstrap-toc.js')
    .pipe(jshint())
    .pipe(jshint.reporter('default'))
    .pipe(jshint.reporter('fail'));
});

gulp.task('test', function () {
  return gulp.src('test/index.html')
    .pipe(mochaPhantomJS());
});

gulp.task('watch', function() {
  gulp.watch('bootstrap-toc.js', ['js-lint', 'test']);
  gulp.watch('test/*', ['test']);
});

gulp.task('build', ['build-css', 'build-js']);

gulp.task('default', ['build', 'js-lint', 'test']);
