var $fixture = $('#mocha-fixture');

afterEach(function() {
  $fixture.empty();
});

describe('Toc', function() {
  describe('.helpers', function() {
    describe('.generateNavItem()', function() {
      it("uses text within the element by default", function() {
        var heading = $('<h1>foo</h1>')[0];
        var $navItem = Toc.helpers.generateNavItem(heading);
        expect($navItem.text()).to.eql('foo');
      });

      it("uses text specified as a data-toc-text attribute", function() {
        var heading = $('<h1 data-toc-text="foo">bar</h1>')[0];
        var $navItem = Toc.helpers.generateNavItem(heading);
        expect($navItem.text()).to.eql('foo');
      });

      it("keeps the text from within the element escaped", function() {
        var heading = document.createElement('h1');
        heading.innerText = '&lt;script&gt;foo&lt;/script&gt;';
        var navItem = Toc.helpers.generateNavItem(heading)[0];
        expect(navItem.textContent).to.eql('&lt;script&gt;foo&lt;/script&gt;');
      });

      it("keeps the data-toc-text escaped", function() {
        var heading = document.createElement('h1');
        heading.setAttribute('data-toc-text', '&lt;script&gt;foo&lt;/script&gt;')
        heading.innerText = 'bar';
        var $navItem = Toc.helpers.generateNavItem(heading);
        expect($navItem.text()).to.eql('&lt;script&gt;foo&lt;/script&gt;');
      });
    });

    describe('.generateUniqueIdBase()', function() {
      it("uses the text from the element", function() {
        var el = document.createElement('h1');
        el.innerHTML = "Some  tExt- with aidan's /. stuff   "
        var base = Toc.helpers.generateUniqueIdBase(el);
        expect(base).to.eql('some-text-with-aidan-s-stuff');
      });

      it("uses the tag name of the element if there's no text", function() {
        var el = document.createElement('h1');
        var base = Toc.helpers.generateUniqueIdBase(el);
        expect(base).to.eql('h1');
      });
    });

    describe('.generateUniqueId()', function() {
      it("uses the tag name", function() {
        var el = document.createElement('h1');
        var base = Toc.helpers.generateUniqueId(el);
        expect(base).to.eql('h1');
      });

      it("adds a suffix when there's an existing element with that tag", function() {
        $fixture.append('<h1 id="h1"></h1>');

        var el = document.createElement('h1');
        var base = Toc.helpers.generateUniqueId(el);
        expect(base).to.eql('h1-1');
      });
    });

    describe('.getTopLevel()', function() {
      it("returns 1 by default", function() {
        var $scope = $('<div></div>');
        var level = Toc.helpers.getTopLevel($scope);
        expect(level).to.eql(1);
      });

      it("returns the first level with more than one element", function() {
        var $scope = $(
          '<div>' +
            '<h1></h1>' +
            '<h2></h2>' +
            '<h3></h3>' +
            '<h4></h4>' +
            '<h5></h5>' +
            '<h6></h6>' +
            '<h6></h6>' +
          '</div>'
        );
        var level = Toc.helpers.getTopLevel($scope);
        expect(level).to.eql(6);
      });
    });

    describe('.getNavLevel()', function() {
      it("returns the value from the tag", function() {
        var el = document.createElement('h5');
        var level = Toc.helpers.getNavLevel(el);
        expect(level).to.eql(5);
      });
    });
  });

  describe('.init()', function() {
    var $nav;
    beforeEach(function() {
      $nav = $('<nav>');
      $fixture.append($nav);
    });

    it("handles single-level headings", function() {
      $fixture.append(
        '<h1>H1</h1>' +
        '<h1>H1</h1>' +
        '<h1>H1</h1>'
      );

      Toc.init({
        $nav: $nav,
        $scope: $fixture
      });

      expect($nav.html()).to.eql(
        '<ul class="nav">' +
          '<li>' +
            '<a href="#h1">H1</a>' +
          '</li>' +
          '<li>' +
            '<a href="#h1-1">H1</a>' +
          '</li>' +
          '<li>' +
            '<a href="#h1-2">H1</a>' +
          '</li>' +
        '</ul>'
      );
    });

    it("ignores headings with a data-toc-skip attribute", function() {
      $fixture.append(
        '<h1>H1</h1>' +
        '<h1 data-toc-skip>H1</h1>' +
        '<h1>H1</h1>'
      );

      Toc.init({
        $nav: $nav,
        $scope: $fixture
      });

      expect($nav.html()).to.eql(
        '<ul class="nav">' +
          '<li>' +
            '<a href="#h1">H1</a>' +
          '</li>' +
          '<li>' +
            '<a href="#h1-1">H1</a>' +
          '</li>' +
        '</ul>'
      );
    });

    it("handles nested headings", function() {
      $fixture.append(
        '<h1>H1</h1>' +
        '<h2>H2</h2>' +
        '<h3>H3</h3>' +
        '<h4>H4</h4>' +
        '<h2>H2-1</h2>'
      );

      Toc.init({
        $nav: $nav,
        $scope: $fixture
      });

      expect($nav.html()).to.eql(
        '<ul class="nav">' +
          '<li>' +
            '<a href="#h2">H2</a>' +
            '<ul class="nav">' +
              '<li>' +
                '<a href="#h3">H3</a>' +
              '</li>' +
            '</ul>' +
          '</li>' +
          '<li>' +
            '<a href="#h2-1">H2-1</a>' +
          '</li>' +
        '</ul>'
      );
    });

    it("accepts a list of headings as the $scope", function() {
      var $h1 = $('<h1>H1</h1>');
      var $h2 = $('<h2>H2</h2>');
      $fixture.append($h1, $h2);

      Toc.init({
        $nav: $nav,
        $scope: $h1
      });

      expect($nav.html()).to.eql(
        '<ul class="nav">' +
          '<li>' +
            '<a href="#h1">H1</a>' +
          '</li>' +
        '</ul>'
      );
    });
  });
});
