package server.client;

import org.junit.Test;

public class Tree {
    class BinaryHeap{
        int[] element;
        int curr;

        BinaryHeap(int len){
            curr = 0;
            element = new int[len];
        }

        void insert(int e) throws Exception {
            if(curr>=element.length)
                throw new Exception("exceed max length");
            element[curr] = e;
            curr++;
            int pos = curr - 1;
            while (pos > 0){
                int p = (pos - 1) >> 1;
                if(element[p] > element[pos]){
                    int temp = element[pos];
                    element[pos] = element[p];
                    element[p] = temp;
                    pos = p;
                }else{
                    break;
                }

            }
        }

        int deleteMin() throws Exception {
            if(curr <= 0)
                throw new Exception("heap is empty");
            int res = min();
            curr--;
            element[0] = element[curr];
            int pos = 0;
            while (2 * pos + 1< curr) {
                int left = 2 * pos + 1;
                int right = 2 * pos + 2;
                int temp = element[pos];
                if (element[pos] > element[left]
                        && (right > curr || element[left] < element[right])) {
                    element[pos] = element[left];
                    element[left] = temp;
                    pos = left;
                } else if (right < curr && element[pos] > element[right]) {
                    element[pos] = element[right];
                    element[right] = temp;
                    pos = right;
                } else
                    break;
            }
            return res;
        }

        int min(){
            return element[0];
        }



    }

    @Test
    public void testHeap() throws Exception {
        int[] nums = {2, -4, 4, 9,-5, 10, 23, -6, 11};
        BinaryHeap binaryHeap = new BinaryHeap(9);
        for(int num:nums){
            binaryHeap.insert(num);
        }
        while (binaryHeap.curr > 0){
            System.out.println(binaryHeap.deleteMin());
        }

    }



}
