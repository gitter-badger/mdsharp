var path = require('path'),
    gulp = require('gulp'),
    shell = require('gulp-shell');

var xunitRunnerPath = path.resolve(__dirname, 'packages/xunit.runner.console.2.1.0/tools/xunit.console.exe');

gulp.task('default', ['build']);

gulp.task('build', ['nuget-restore'], function () {
  return gulp.src('*.sln', { read: false })
    .pipe(shell('xbuild'));
});

gulp.task('build-release', ['nuget-restore'], function () {
  return gulp.src('*.sln', { read: false })
    .pipe(shell('xbuild /p:Configuration=Release'));
});

gulp.task('nuget-restore', ['init'], function () {
  return gulp.src('*.sln')
    .pipe(shell('nuget restore <%= file.path %>', {
      cwd: path.resolve(__dirname, 'bin')
    }));
});

gulp.task('test', ['build-release'], function () {
  return gulp.src('**/bin/Release/*.Tests.dll', { read: false })
    .pipe(shell([
      'mono <%= xunit %> <%= file.path %>'
    ], {
      cwd: '<% dirname(file.path) %>',
      templateData: {
        xunit: xunitRunnerPath,
        dirname: path.dirname
      }
    }));
});

gulp.task('init', function () {
  return gulp.src('init/*')
  .pipe(shell('<%= file.path %>', {
    cwd: path.resolve(__dirname)
  }));
});
