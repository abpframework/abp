var abp = abp || {};
(function () {
    abp.ui = abp.ui || {};
    abp.ui.extensions = abp.ui.extensions || {};

    abp.ui.extensions.ActionList = function () {
        return new abp.utils.common.LinkedList();
    };

    abp.ui.extensions.ColumnList = function () {
        return new abp.utils.common.LinkedList();
    };

    abp.ui.extensions.entityActions = (function () {
        var _callbackLists = {};

        function _get(name) {
            var callbackList = _callbackLists[name];

            if (!callbackList) {
                callbackList = _callbackLists[name] = [];
            }

            return {
                addContributor: _addContributor,
                get actions() {
                    return _getActions();
                }
            };

            function _addContributor(contributeCallback, order) {
                if (order === undefined || order >= callbackList.length) {
                    callbackList.push(contributeCallback);
                } else if (order === 0) {
                    callbackList.unshift(contributeCallback);
                } else {
                    callbackList.splice(order, 0, contributeCallback);
                }
            }

            function _getActions() {
                var actionList = new abp.ui.extensions.ActionList();

                callbackList.forEach(function (callback) {
                    callback(actionList);
                });

                return actionList;
            }
        }

        return {
            get: _get
        };
    })();

    abp.ui.extensions.tableColumns = (function () {
        var _callbackLists = {};

        function _get(name) {
            var callbackList = _callbackLists[name];

            if (!callbackList) {
                callbackList = _callbackLists[name] = [];
            }

            return {
                addContributor: _addContributor,
                get columns() {
                    return _getColumns();
                }
            };

            function _addContributor(contributeCallback, order) {
                if (order === undefined || order >= callbackList.length) {
                    callbackList.push(contributeCallback);
                } else if (order === 0) {
                    callbackList.unshift(contributeCallback);
                } else {
                    callbackList.splice(order, 0, contributeCallback);
                }
            }

            function _getColumns() {
                var columnList = new abp.ui.extensions.ColumnList();

                callbackList.forEach(function (callback) {
                    callback(columnList);
                });

                return columnList;
            }
        }

        return {
            get: _get
        };
    })();

    function initializeObjectExtensions() {

        function localizeDisplayName(propertyName, displayName) {
            if (displayName && displayName.name) {
                return abp.localization.localize(displayName.name, displayName.resource);
            }

            if (abp.localization.isLocalized('DisplayName:' + propertyName)) {
                return abp.localization.localize('DisplayName:' + propertyName);
            }

            return abp.localization.localize(propertyName);
        }

        function configureTableColumns(tableName, columnConfigs) {
            abp.ui.extensions.tableColumns.get(tableName)
                .addContributor(
                    function (columnList) {
                        columnList.addManyTail(columnConfigs);
                    }
                );
        }

        function getTableProperties(objectConfig) {
            var propertyNames = Object.keys(objectConfig.properties);
            var tableProperties = [];
            for (var i = 0; i < propertyNames.length; i++) {
                var propertyName = propertyNames[i];
                var propertyConfig = objectConfig.properties[propertyName];
                if (propertyConfig.ui.onTable.isVisible) {
                    tableProperties.push({
                        name: propertyName,
                        config: propertyConfig
                    });
                }
            }

            return tableProperties;
        }

        function convertPropertiesToColumnConfigs(properties) {
            var columnConfigs = [];

            for (var i = 0; i < properties.length; i++) {
                var tableProperty = properties[i];
                columnConfigs.push({
                    title: localizeDisplayName(tableProperty.name, tableProperty.config.displayName),
                    data: "extraProperties." + tableProperty.name,
                    orderable: false
                });
            }

            return columnConfigs;
        }

        function configureEntity(moduleName, entityName, entityConfig) {
            var tableProperties = getTableProperties(entityConfig);

            if (tableProperties.length > 0) {
                var tableName = abp.utils.toCamelCase(moduleName) + "." + abp.utils.toCamelCase(entityName);
                var columnConfigs = convertPropertiesToColumnConfigs(tableProperties);
                configureTableColumns(
                    tableName,
                    columnConfigs
                );
            }
        }

        function configureModule(moduleName, moduleConfig) {
            var entityNames = Object.keys(moduleConfig.entities);
            for (var i = 0; i < entityNames.length; i++) {
                configureEntity(
                    moduleName,
                    entityNames[i],
                    moduleConfig.entities[entityNames[i]]
                );
            }
        }

        var moduleNames = Object.keys(abp.objectExtensions.modules);

        for (var i = 0; i < moduleNames.length; i++) {
            configureModule(
                moduleNames[i],
                abp.objectExtensions.modules[moduleNames[i]]
            );
        }
    }

    abp.event.on('abp.configurationInitialized', function () {
        initializeObjectExtensions();
    });

})();
