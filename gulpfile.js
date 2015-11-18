var path = require('path'),
    gulp = require('gulp'),
    notify = require('gulp-notify'),
    shell = require('gulp-shell');

var xunitRunnerPath = path.resolve(__dirname, 'packages/xunit.runner.console.2.1.0/tools/xunit.console.exe');

gulp.task('default', ['build']);

gulp.task('build', ['nuget-restore'], function () {
  return gulp.src('*.sln', { read: false })
    .pipe(shell('xbuild'));
});

gulp.task('build-release', ['nuget-restore'], function () {
  var xbuildShell = shell('xbuild /p:Configuration=Release')
    .on('error', notify.onError("Build failed: <%= error.message %>"));

  return gulp.src('*.sln', { read: false })
    .pipe(xbuildShell);
});

gulp.task('nuget-restore', ['init'], function () {
  return gulp.src('*.sln')
    .pipe(shell('nuget restore <%= file.path %>', {
      cwd: path.resolve(__dirname, 'bin')
    }));
});

gulp.task('test', ['build-release'], function () {
  var shellOptions = {
    cwd: '<% dirname(file.path) %>',
    templateData: {
      xunit: xunitRunnerPath,
      dirname: path.dirname
    }
  };

  var monoShell = shell('mono <%= xunit %> <%= file.path %>', shellOptions)
    .on('error', notify.onError("Tests failed: <%= error.message %>"));

  return gulp.src('**/bin/Release/*.Tests.dll', { read: false })
    .pipe(monoShell)
    .pipe(notify("Tests passed!"));
});

gulp.task('watch', function () {
  gulp.watch(
    // HACK: ?(_) is to evade the glob v5 ignore pattern: https://www.npmjs.com/package/glob#comments-and-negation
    ['?(_)!(packages|node_modules|bin|init)/!(bin|obj){,/*,/**/*}.*', '*.sln'],
    { cwd: __dirname },
    ['test']
  );
});

gulp.task('init', function () {
  return gulp.src('init/*')
  .pipe(shell('<%= file.path %>', {
    cwd: path.resolve(__dirname)
  }));
});
