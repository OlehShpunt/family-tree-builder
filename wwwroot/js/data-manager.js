class DataManager {
  static addChild(cName, cGender) {
    if (!window.currentSelectedNode) {
      alert("Please select a child's parent first!");
      return;
    }

    console.log(cGender);

    const child = {
      id: this._getRandomId(),
      gender: cGender.toLowerCase(),
      name: cName,
      mid: null,
      fid: null,
    };

    if (window.currentSelectedNode.gender == "male") {
      child.fid = window.currentSelectedNode.id;
      child.mid = window.currentSelectedNode.pids[0];
    } else {
      child.mid = window.currentSelectedNode.id;
      child.fid = window.currentSelectedNode.pids[0];
    }

    // Take data from previous render and combine with new added child
    this.updateLastData([child, ...window.lastData]);

    this.initializeTreeRerender();
  }

  static addParents(mName, fName) {
    if (!window.currentSelectedNode) {
      alert("Please select a person first!");
      return;
    }

    const mother = {
      id: this._getRandomId(),
      gender: "female",
      name: mName,
      pids: null, // Specified later after father initialization
    };
    const father = {
      id: this._getRandomId(),
      gender: "male",
      name: fName,
      pids: null, // Specified later
    };

    // Connect father and mother as partners
    mother.pids = [father.id];
    father.pids = [mother.id];

    // Update the child's mother and father IDs
    window.lastData.find((node) => {
      if (node.id == window.currentSelectedNode.id) {
        node.mid = mother.id;
        node.fid = father.id;
        // TODO: track the IDs of previous parent nodes
      }
    });
    // TODO: remove previous parent nodes using the tracked IDs

    // Take data from previous render and combine with new added parents
    this.updateLastData([mother, father, ...window.lastData]);

    this.initializeTreeRerender();
  }

  deleteSelectedPerson() {
    const selectedNode = window.currentSelectedNode;

    if (!selectedNode) {
      alert("Please select a person to delete!");
      return;
    }

    if (
      !confirm(
        `Are you sure you want to delete "${selectedNode.name}" and all their descendants?`
      )
    ) {
      return;
    }

    // This removes the node and all its children automatically
    tree.removeNode(selectedNode.id);
  }

  static _getRandomId() {
    return Math.floor(Math.random() * 10000) + 1; // 1 to 10000 inclusive
  }

  static updateLastData(value) {
    window.lastData = value;
  }

  // NOTE: window.lastData needs to be udpated before calling this method
  static initializeTreeRerender() {
    const container = document.getElementById("treeContainer");

    container.innerHTML = "";
    window.familyTreeInstance?.destroy();

    window.familyTreeInstance = new FamilyTree(
      document.getElementById("treeContainer"),
      {
        mode: "light",
        nodeBinding: {
          field_0: "name",
          field_1: "title",
          img_0: "photoUrl",
        },
      }
    );

    window.currentSelectedNode = null;

    // Attach listener
    window.familyTreeInstance.onNodeClick((args) => {
      const node = args.node; // clicked node object
      window.currentSelectedNode = node;
      console.log("Node clicked:", window.currentSelectedNode);
    });

    window.familyTreeInstance.load(window.lastData);
  }
}

window.DataManager = DataManager;
