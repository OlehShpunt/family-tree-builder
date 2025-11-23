class DataLoader {
  constructor(dataUrl) {
    this.tree = null;
    this.dataUrl = dataUrl;
  }

  initialize() {
    const container = document.getElementById("treeContainer");

    if (!container) {
      console.error(`FamilyTree container "treeContainer" not found`);
      return;
    }

    // Clean up previous instance
    if (container.familyTreeInstance) {
      container.familyTreeInstance.destroy();
    }

    this.tree = new FamilyTree(container, {
      mouseScrol: FamilyTree.action.scroll,
      enableSearch: true,
      nodeBinding: {
        field_0: "name",
        field_1: "title",
        img_0: "photoUrl",
      },
    });

    container.familyTreeInstance = this.tree;

    this.loadDatabaseData();
  }

  loadDatabaseData() {
    fetch(this.dataUrl)
      .then((r) => {
        if (!r.ok) throw new Error(`HTTP ${r.status}`);
        return r.json();
      })
      .then((data) => {
        this.tree.load(data);
      })
      .catch((err) => {
        console.error("Failed to load family tree data:", err);
        alert("Could not load family data");
      });
  }
}

// Make globally available
window.DataLoader = DataLoader;
