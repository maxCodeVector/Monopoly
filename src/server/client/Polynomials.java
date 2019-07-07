package server.client;

import org.junit.Test;

public class Polynomials implements Cloneable{

    private Node head;
    private Node tail;

    public class Node{
        float coef;
        int exp;
        Node next;

        Node(float coef, int exp){
            this.coef = coef;
            this.exp = exp;
            next = null;
        }
    }

    private void addItem(Node node){
        if(head==null){
            head = node;
            tail = node;
        }else {
            while (tail.exp != node.exp-1){
                tail.next = new Node(0, tail.exp+1);
                tail = tail.next;
            }
            tail.next = node;
            tail = node;
        }
    }

    private void addItemByDetails(float coef, int exp){
        Node node = new Node(coef, exp);
        addItem(node);
    }

    private static Polynomials add(Polynomials p1, Polynomials p2){
        Polynomials res2 = new Polynomials();
        Node p1item = p1.head, p2item = p2.head;
        while (p1item!=null || p2item != null){
            if(p1item==null){
                while (p2item!=null){
                    res2.addItemByDetails(p2item.coef, p2item.exp);
                    p2item = p2item.next;
                }
                return res2;
            }
            if(p2item==null){
                while (p1item!=null){
                    res2.addItemByDetails(p1item.coef, p1item.exp);
                    p1item = p1item.next;
                }
                return res2;
            }
            // do something
            assert p1item.exp == p2item.exp;
            res2.addItemByDetails(p1item.coef+p2item.coef, p1item.exp);
            p1item = p1item.next;
            p2item = p2item.next;
        }
        return res2;
    }

    @Override
    public String toString() {
        StringBuilder sb = new StringBuilder();
        Node tempItem = head;
        while (tempItem!=null){
            sb.append(String.format("%.2fx^%d+", tempItem.coef, tempItem.exp));
            tempItem = tempItem.next;
        }
        return sb.toString();
    }

    private static Polynomials mul(Polynomials p1, Polynomials p2){
        Polynomials res = new Polynomials();
        res.addItemByDetails(0, 0);
        Node p1item = p1.head, p2item;
        Node tempItem;
        while (p1item!=null){
            tempItem = res.head;
            p2item = p2.head;
            while (tempItem==null || tempItem.exp!=p1item.exp){
                if(tempItem==null){
                    res.addItemByDetails(0, res.tail.exp+1);
                    tempItem = res.tail;
                }else
                    tempItem = tempItem.next;
            }
            while (p2item!=null){
                if(tempItem==null){
                    res.addItemByDetails(p1item.coef * p2item.coef, p1item.exp+p2item.exp);
                }else {
                    tempItem.coef += p1item.coef * p2item.coef;
                    tempItem = tempItem.next;
                }
                p2item = p2item.next;
            }
            p1item = p1item.next;
        }
        return res;
    }

    @Test
    public void testAdd(){
        Polynomials p1 = new Polynomials();
        p1.addItemByDetails(1, 0);
        p1.addItemByDetails(2, 1);
        p1.addItemByDetails(3, 2);
        p1.addItemByDetails(4, 3);
        Polynomials p2 = new Polynomials();
        p2.addItemByDetails(2, 0);
        p2.addItemByDetails(3, 1);
        p2.addItemByDetails(1, 2);
//        p2.addItemByDetails(4, 3);
        Polynomials p3 = add(p1, p2);
        assert p3.tail.coef==4 && p3.tail.exp == 3;
    }

    @Test
    public void testMul(){
        Polynomials p1 = new Polynomials();
        p1.addItemByDetails(1, 0);
        p1.addItemByDetails(2, 1);
        p1.addItemByDetails(3, 2);
        p1.addItemByDetails(4, 3);
        Polynomials p2 = new Polynomials();
        p2.addItemByDetails(2, 0);
        p2.addItemByDetails(1, 2);
//        p2.addItemByDetails(4, 3);
        Polynomials p3 = mul(p1, p2);
        assert p3.tail.coef==4 && p3.tail.exp == 5;
    }
}
