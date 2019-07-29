package server.client;

public class Tree {

    public static interface Heap {
        void insert(int e) throws Exception;
        int deleteMin() throws Exception;
    }

    public static interface BinarySearchTree{
        server.client.BinarySearchTree.Node predecessorQuery(int q);
        server.client.BinarySearchTree.Node successorQuery(int q);
        void insert(int e);
        void delete(int e);
    }



}
