var gulp = require('gulp'),
    shell = require('gulp-shell'),
    nuget = require('gulp-nuget-packages');

gulp.task('default', ['nuget-restore', 'build', 'unit-test'])

gulp.task('build', function () {
  return gulp.src('*.sln', { read: false })
    .pipe(shell('xbuild'));
});

gulp.task('nuget-restore', function () {
  return gulp.src('').pipe(nuget({
    solutionPath: '../..',
    sln: 'MdSharp.sln',
    nugetPath: 'node_modules/gulp-nuget-packages'
  }));
});

gulp.task('unit-test', ['build'], function () {
  return gulp.src('')
    .pipe(shell([
      // HACK: This has got to be super brittle.
      'mono ../../../packages/xunit.runner.console.2.1.0/tools/xunit.console.exe MdSharp.Tests.dll'
    ], {
      cwd: 'MdSharp.Tests/bin/Debug'
    }));
});
