"use strict";

var gulp = require("gulp"),
    path = require('path'),
    copyResources = require('./node_modules/@abp/aspnetcore.mvc.ui/gulp/copy-resources.js');

copyResources.init(path.resolve('./abp.resourcemapping.js'));

gulp.task('default', [copyResources.taskName], function () {

});