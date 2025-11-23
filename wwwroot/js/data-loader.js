class DataLoader {
  constructor(dataUrl) {
    this.tree = null;
    this.dataUrl = dataUrl;
  }

  async initialize() {
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
      mouseScroll: FamilyTree.action.scroll,
      enableSearch: true,
      nodeBinding: {
        field_0: "name",
        field_1: "title",
        img_0: "photoUrl",
      },
    });

    window.familyTreeInstance = this.tree;
    window.currentSelectedNode = null;

    // Attach listener
    this.tree.onNodeClick((args) => {
      const node = args.node; // clicked node object
      window.currentSelectedNode = node;
      console.log("Node clicked:", window.currentSelectedNode);
    });

    // Load the tree data
    await this.loadDatabaseData();
  }

  async loadDatabaseData() {
    try {
      const response = await fetch(this.dataUrl);
      if (!response.ok) throw new Error(`HTTP ${response.status}`);
      const data = await response.json();
      this.tree.load(data);
      window.lastData = data;
    } catch (err) {
      console.error("Failed to load family tree data:", err);
      alert("Could not load family data");
    }
  }
}

window.DataLoader = DataLoader;
