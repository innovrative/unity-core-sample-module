mergeInto(LibraryManager.library, {
  Hello: function () {
    window.alert("Hello");
  },

  HelloString: function (str) {
    window.alert(UTF8ToString(str));
  },

  logErrors: function (str) {
    console.log(str);
  },
});
