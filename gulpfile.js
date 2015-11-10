var path = require('path'),
    gulp = require('gulp'),
    shell = require('gulp-shell'),
    nuget = require('gulp-nuget-packages');

var xunitRunnerPath = path.resolve(__dirname, 'packages/xunit.runner.console.2.1.0/tools/xunit.console.exe');

gulp.task('default', ['build'])

gulp.task('build', ['nuget-restore'], function () {
  return gulp.src('*.sln', { read: false })
    .pipe(shell('xbuild'));
});

gulp.task('build-release', ['nuget-restore'], function () {
  return gulp.src('*.sln', { read: false })
    .pipe(shell('xbuild /p:Configuration=Release'));
});

gulp.task('nuget-restore', function () {
  return gulp.src('').pipe(nuget({
    solutionPath: '../..',
    sln: 'MdSharp.sln',
    nugetPath: 'node_modules/gulp-nuget-packages'
  }));
});

gulp.task('test', ['build-release'], function () {
  return gulp.src('**/bin/Release/MdSharp.Tests.dll', { read: false })
    .pipe(shell([
      'mono <%= xunit %> MdSharp.Tests.dll'
    ], {
      cwd: 'MdSharp.Tests/bin/Release',
      templateData: { xunit: xunitRunnerPath }
    }));
});
