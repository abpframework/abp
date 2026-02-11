$(function () {
    var l = abp.localization.getResource('CmsKit');

    let filterButton = $('.favorite-button');
    let filterOnFavorites = filterButton.attr('filter-on-favorites');
    const loginModal = new abp.ModalManager(abp.appPath + 'CmsKit/Shared/Modals/Login/LoginModal');

    $('.favorite-button').on('click', function () {
        if (!abp.currentUser.isAuthenticated) {
            const currentPageRoute = window.location.pathname;
            loginModal.open({ message: l("FavoritesFilterMessage"), returnUrl: currentPageRoute });
            return;
        }

        let currentUrl = new URL(window.location.href);
        let searchParams = currentUrl.searchParams;

        // Toggle the 'filterOnFavorites' parameter
        if (filterOnFavorites) {
            searchParams.delete('filterOnFavorites');
        } else {
            searchParams.set('filterOnFavorites', 'true');
        }

        window.location.href = currentUrl.pathname + '?' + searchParams.toString();
    });
});