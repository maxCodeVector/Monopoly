package server.client;

public class ArrayHeap implements Tree.Heap {
    int[] element;
    int curr;

    public ArrayHeap(int len){
        curr = 0;
        element = new int[len];
    }

    public ArrayHeap(int[] nums){
        curr = nums.length;
        element = new int[nums.length];
        System.arraycopy(nums, 0, element, 0, nums.length);
        for(int i=nums.length-1;i>=0;i--){
            rootFix(i);
        }
    }

    private void rootFix(int pos){
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
    }

    public void insert(int e) throws Exception {
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

    public int deleteMin() throws Exception {
        if(curr <= 0)
            throw new Exception("heap is empty");
        int res = min();
        curr--;
        element[0] = element[curr];
        rootFix(0);
        return res;
    }

    private int min(){
        return element[0];
    }


}
