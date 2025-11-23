class DataManager {
  static addChild(child) {
    // Set parent ids
    const currentSelectedNode = getCurrentSelectedNode();

    if (!currentSelectedNode) {
      alert("Please select a person first!");
      return;
    }

    child.id = _getRandomId();

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

  static addParents(mother, father) {
    const currentSelectedNode = getCurrentSelectedNode();

    if (!currentSelectedNode) {
      alert("Please select a person first!");
      return;
    }

    mother.id = _getRandomId();
    father.id = _getRandomId();

    mother.pids = [father.id];
    father.pids = [mother.id];
  }

  static editPerson(updatedData) {
    const selectedNode = getCurrentSelectedNode();

    if (!selectedNode) {
      alert("Please select a person first!");
      return;
    }

    const updatedNode = {
      id: selectedNode.id,
      name: updatedData.name || selectedNode.name,
      gender: updatedData.gender || selectedNode.gender,
    };

    tree.updateNode(updatedNode);
  }

  deleteSelectedPerson() {
    const selectedNode = getCurrentSelectedNode();

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

  static getCurrentSelectedNode() {
    const tree = document.getElementById("treeContainer");
    const selected = tree.getSelectedNodes();
    return selected.length > 0 ? selected[0] : null;
  }
}
