package server.client;

import java.util.*;

public class testERROR {

    private interface PrintN{
        void print(int num);
    }

    private class BB{
        PrintN p = new PrintN() {
            @Override
            public synchronized void print(int num) {
                for(int i=0;i<num;i++){
                    System.out.print(i);
                }
                System.out.println();
            }
        };
    }

    private class PN implements Runnable{
        BB bb = new BB();
        int num;

        PN(int num){
            this.num = num;
        }


        @Override
        public void run() {
            bb.p.print(num);
        }

    }




    public static void test() throws Exception {
        throw new Exception();
    }


    public double findMedianSortedArrays(int[] nums1, int[] nums2) {




        return 0;
    }


    public static void main(String[] args){

        testERROR t = new testERROR();
        System.out.println(t.findMedianSortedArrays(new int[]{1, 2}, new int[]{3, 4}));

//        PN pn = new testERROR().new PN(50);
//        Thread t1 = new Thread(pn);
//        Thread t2 = new Thread(pn);
//        t1.start();
//        t2.start();
    }

}
