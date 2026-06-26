(function() {
  // Render mermaid blocks inside a container (only unprocessed ones)
  function renderMermaid(container) {
    var blocks = container.querySelectorAll('pre.mermaid:not([data-processed])');
    if (blocks.length > 0) {
      mermaid.run({nodes: blocks});
    }
  }
  // Tab switching
  document.querySelectorAll('.nav-tab').forEach(function(btn) {
    btn.addEventListener('click', function() {
      document.querySelectorAll('.nav-tab').forEach(function(b) { b.classList.remove('active'); });
      document.querySelectorAll('.tab-content').forEach(function(c) { c.classList.remove('active'); });
      btn.classList.add('active');
      var target = document.getElementById(btn.getAttribute('data-tab-id'));
      if (target) {
        target.classList.add('active');
        renderMermaid(target);
      }
      history.replaceState(null, '', '#' + btn.getAttribute('data-tab-id'));
    });
  });
  // Hash-based tab activation
  function activateFromHash() {
    var hash = window.location.hash.replace('#', '');
    if (hash) {
      var btn = document.querySelector('.nav-tab[data-tab-id="' + hash + '"]');
      if (btn) btn.click();
    }
  }
  activateFromHash();
  window.addEventListener('hashchange', activateFromHash);
  // Collapsible sections
  document.querySelectorAll('.collapsible-header').forEach(function(h) {
    function toggle() {
      h.classList.toggle('expanded');
      h.nextElementSibling.classList.toggle('show');
    }
    h.addEventListener('click', toggle);
    h.addEventListener('keydown', function(e) {
      if (e.key === 'Enter' || e.key === ' ') { e.preventDefault(); toggle(); }
    });
  });
  // Render mermaid in the initially active tab
  var activeTab = document.querySelector('.tab-content.active');
  if (activeTab) renderMermaid(activeTab);
})();
