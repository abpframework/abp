"use strict";

(function () {

    var taskName = 'aspnetcore.mvc.ui.copy-resources';

    var gulp = require("gulp"),
        merge = require("merge-stream"),
        rimraf = require("rimraf"),
        path = require("path"),
        extendObject = require('extend-object');

    function init(rootMappingFilePath) {
        var resourceMapping = buildResourceMapping(rootMappingFilePath);

        function replaceAliases(text) {
            if (!resourceMapping.aliases) {
                return text;
            }

            for (var alias in resourceMapping.aliases) {
                if (!resourceMapping.aliases.hasOwnProperty(alias)) {
                    continue;
                }

                text = replaceAll(text, alias, resourceMapping.aliases[alias]);
            }

            return text;
        }

        function replaceAll(text, search, replacement) {
            return text.replace(new RegExp(search, 'g'), replacement);
        }

        function cleanFiles() {
            if (resourceMapping.clean) {
                for (var i = 0; i < resourceMapping.clean.length; i++) {
                    rimraf.sync(replaceAliases(resourceMapping.clean[i]) + '/**/*', { force: true });
                }
            }
        }

        function buildResourceMapping(mappingFilePath) {
            var resourcemapping = require(mappingFilePath);

            if (resourcemapping.imports && resourcemapping.imports.length) {

                var aliases = {};
                var mappings = {};

                for (var i = 0; i < resourcemapping.imports.length; i++) {

                    var importedMappingFilePath = path.join(path.dirname(mappingFilePath), resourcemapping.imports[i]);
                    var importedResourceMapping = buildResourceMapping(importedMappingFilePath);
                    extendObject(aliases, importedResourceMapping.aliases);
                    extendObject(mappings, importedResourceMapping.mappings);
                }

                extendObject(aliases, resourcemapping.aliases);
                extendObject(mappings, resourcemapping.mappings);

                resourcemapping.aliases = aliases;
                resourcemapping.mappings = mappings;
            }

            return resourcemapping;
        }

        gulp.task(taskName,
            function () {
                cleanFiles();

                var tasks = [];

                for (var mapping in resourceMapping.mappings) {
                    if (resourceMapping.mappings.hasOwnProperty(mapping)) {
                        var source = replaceAliases(mapping);
                        var destination = replaceAliases(resourceMapping.mappings[mapping]);
                        tasks.push(
                            gulp.src(source).pipe(gulp.dest(destination))
                        );
                    }
                }

                return merge(tasks);
            });
    }

    module.exports = {
        taskName: taskName,
        init: init
    };
})();