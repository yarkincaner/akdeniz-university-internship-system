'use client';

import React, { useState } from 'react';
import data from '../../treeData.json'; 
import { ChevronDown , ChevronRight } from 'lucide-react';

interface TreeNode {
  id: number;
  name: string;
  children?: TreeNode[];
}

const TreeView: React.FC = () => {
  const [treeData, setTreeData] = useState<TreeNode[]>(data);
  const [nodeState, setNodeState] = useState<{ [key: string]: boolean }>({});

  const toggleNode = (parentId: number | null, childId: number) => {
    const key = parentId ? `${parentId}_${childId}` : `${childId}`;
    setNodeState(prevState => ({
      ...prevState,
      [key]: !prevState[key],
    }));

    if (parentId !== null) {
      const currentNode = treeData.find(node => node.id === childId);
      if (currentNode && currentNode.children) {
        currentNode.children.forEach(child => {
          const childKey = `${parentId}_${child.id}`;
          if (child.id !== childId) {
            setNodeState(prevState => ({
              ...prevState,
              [childKey]: false,
            }));
          }
        });
      }
    }
  };

  const renderTreeNode = (node: TreeNode, parentId: number | null) => {
    const isOpen = nodeState[parentId ? `${parentId}_${node.id}` : `${node.id}`];
    
    return (
      <div key={node.id} className="ml-4">
        <div
          className={`cursor-pointer font-medium flex items-center mb-4 rounded-md px-4 py-2 focus:bg-blue-100 hover:bg-gray-200 transition duration-100 ${
            node.children ? 'hover:bg-gray-200' : ''
          }`}
          onClick={() => toggleNode(parentId, node.id)}
          tabIndex={0}
        >
          {node.children && (isOpen ? <ChevronDown className="mr-2" /> : <ChevronRight className="mr-2" />)}
          <span>{node.name}</span>
        </div>
        {isOpen && node.children && node.children.map(child => (
          <div key={child.id} className="ml-4">
            {renderTreeNode(child, node.id)}
          </div>
        ))}
      </div>
    );
  };

  return (
    <div className="p-4">
      {treeData.map(node => (
        <div key={node.id} className="mb-4">
          {renderTreeNode(node, null)}
        </div>
      ))}
    </div>
  );
};

export default TreeView;