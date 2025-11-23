class DataManager {
  static addChild(child) {
    // Set parent ids
    const currentSelectedNode = window.currentSelectedNode;

    if (!currentSelectedNode) {
      alert("Please select a person first!");
      return;
    }

    child.id = this._getRandomId();

    if (currentSelectedNode.gender.toLowerCase() == "male") {
      child.fid = currentSelectedNode.id;
      child.mid = currentSelectedNode.pids[0];
    } else {
      child.mid = currentSelectedNode.id;
      child.fid = currentSelectedNode.pids[0];
    }

    const tree = document.getElementById("treeContainer");
    const allPersonNodes = tree.get();
    tree.load([...allPersonNodes, child]);
  }

  static addParents(mName, fName) {
    console.log("Adding parents with names ", mName, " and ", fName);

    console.log(window.currentSelectedNode);

    if (!currentSelectedNode) {
      alert("Please select a person first!");
      return;
    }

    var mother = {};
    var father = {};

    mother.id = this._getRandomId();
    father.id = this._getRandomId();
    mother.gender = "female";
    father.gender = "male";
    mother.name = mName;
    father.name = fName;

    mother.pids = [father.id];
    father.pids = [mother.id];

    const selectedNodeId = window.currentSelectedNode.id;
    console.log("Last data 1 element: ", window.lastData[0]);
    const allNodes = Object.values(window.familyTreeInstance);
    console.log("allNodes = ", allNodes);
    console.log("tree global instance: ", window.familyTreeInstance);

    console.log("After calculations, the selectedNodeId is ", selectedNodeId);

    console.log("M: ", mother, "F: ", father);

    window.lastData.find((node) => {
      if (node.id == selectedNodeId) {
        node.mid = mother.id;
        node.fid = father.id;
      }
    });

    this.updateLastData([...window.lastData, mother, father]);

    console.log(window.lastData[0]);
    console.log(window.lastData[1]);
    console.log(window.lastData[2]);
    console.log(window.lastData[3]);
    console.log(window.lastData[4]);

    window.familyTreeInstance.load(window.lastData);
  }

  static editPerson(updatedData) {
    console.log("NAMEIS: ", window.selectedNode.name);

    if (!window.selectedNode) {
      alert("Please select a person first!");
      return;
    }

    const updatedNode = {
      id: window.selectedNode.id,
      name: updatedData.name || window.selectedNode.name,
      gender: updatedData.gender || window.selectedNode.gender,
    };

    tree.updateNode(updatedNode);
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
}

window.DataManager = DataManager;
