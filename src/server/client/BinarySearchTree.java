package server.client;

import org.junit.Test;

import java.util.LinkedList;
import java.util.Queue;

public class BinarySearchTree implements Tree.BinarySearchTree {

    Node root;

    class Node {
        Node father;
        Node left;
        Node right;
        int e;

        Node(int e) {
            this.e = e;
        }
    }

    @Override
    public Node predecessorQuery(int q) {
        Node p = null;
        Node u = root;
        while (true) {
            if (u == null)
                return p;
            else if (u.e == q) {
                p = u;
                return p;
            } else if (u.e > q) {
                u = u.left;
            } else {
                p = u;
                u = u.right;
            }
        }
    }

    @Override
    public Node successorQuery(int q) {
        return successorQHelper(q, root);
    }

    private Node successorQHelper(int q, Node u){
        Node p=null;
        while (true) {
            if (u == null)
                return p;
            else if (u.e == q) {
                p = u;
                return p;
            } else if (u.e > q) {
                p = u;
                u = u.left;
            } else {
                u = u.right;
            }
        }
    }

    @Override
    public void insert(int e) {
        if (root == null) {
            root = new Node(e);
            return;
        }
        Node u = root;
        while (true) {
            if (e < u.e) {
                if (u.left != null)
                    u = u.left;
                else {
                    u.left = new Node(e);
                    u.left.father = u;
                    return;
                }
            } else {
                if (u.right != null)
                    u = u.right;
                else {
                    u.right = new Node(e);
                    u.right.father = u;
                    return;
                }
            }
        }
    }

    @Override
    public void delete(int e) {
        Node obj = predecessorQuery(e);
        if(obj==null || obj.e != e)
            return;
        remove(obj);
    }


    void remove(Node node){
        if(node.left==null && node.right ==null){
            if(node==node.father.left)
                node.father.left = null;
            else
                node.father.right = null;
        }else if(node.right!=null){
            Node v = successorQHelper(node.e, node.right);
            node.e = v.e;
            if(v.left==null && v.right==null){
                remove(v);
            }else {
                // there must node v only has a right child
                v.e = v.right.e;
                v.left = v.right.left;
                v.right = v.right.right;
            }
        }else{
            // there must node only has a left child
            node.right = node.left.right;
            node.e = node.left.e;
            node.left = node.left.left;
        }
    }

    @Test
    public void test() {
        int[] S = {40, 15, 73, 10, 30, 60, 80, 3, 20};
        for (int s : S)
            insert(s);
        assert successorQuery(23).e == 30;
        assert successorQuery(15).e == 15;
        assert successorQuery(81) == null;
    }

    @Test
    public void test2() {
        int[] S = {40, 15, 73, 10, 30, 60, 80, 3, 20};
        for (int s : S)
            insert(s);
        assert predecessorQuery(23).e == 20;
        assert predecessorQuery(15).e == 15;
        assert predecessorQuery(2) == null;
    }

    @Override
    public String toString(){
        StringBuilder sb = new StringBuilder();
        Queue<Node> queue = new LinkedList<>();
        queue.add(root);
        while (!queue.isEmpty()){
            Node node = queue.poll();
            sb.append(node.e).append(" ");
            if(node.left!=null)
                queue.add(node.left);
            if(node.right!=null)
                queue.add(node.right);
        }
        return sb.toString();
    }



    @Test
    public void test3() {
        int[] S = {30, 15, 40, 10, 20, 35, 73, 3, 36, 60};
        for (int s : S)
            insert(s);
        delete(60);
    }

    @Test
    public void test4() {
        int[] S = {30, 15, 40, 10, 20, 35, 73, 3, 36, 60};
        for (int s : S)
            insert(s);
        delete(40);
    }

    @Test
    public void test5() {
        int[] S = {30, 15, 40, 10, 20, 35, 73, 3, 36, 60};
        for (int s : S)
            insert(s);
        delete(30);
    }
    @Test
    public void test6() {
        int[] S = {30, 15, 40, 10, 20, 35, 73, 3, 36, 60};
        for (int s : S)
            insert(s);
        delete(73);
    }

}
