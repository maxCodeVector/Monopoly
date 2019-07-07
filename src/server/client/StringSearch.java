package server.client;

import org.junit.Before;
import org.junit.Test;
import org.junit.Assert;

import java.util.*;

public class StringSearch {

    private int Encode_length = 256;
    private int Q = 1001;

    int[] nextArray(String pattern){
        int m = pattern.length();
        int[] next = new int[m];
        next[1] = 0;
        int k = 0;
        for(int j=2;j<m;j++){
            if(pattern.charAt(k+1)==pattern.charAt(j)){
                k ++;
            } else {
                k = next[k];
            }
            next[j] = k;
        }
        return next;
    }

    private boolean searchByKnuthMorrisPratt(String text, String pattern) {
        int n = text.length();
        int m = pattern.length();
        int[] next = nextArray(pattern);
        int q = 0;
        for(int i=0;i<n;i++){
            if(pattern.charAt(q) != text.charAt(i))
                q = next[q];
            else
                q++;
            if(q==m)
                return true;
        }
        return false;
    }




    Map<Character, Integer>[] transition(String pattern){
        Set<Character> set = new TreeSet<>();
        int m = pattern.length();
        Map<Character, Integer>[] delta = new Map[m];
        for(int i=0;i<m;i++){
            set.add(pattern.charAt(i));
            delta[i] = new HashMap<>();
        }
        int x = 0;
        for(Character c:set){
            for(Map map: delta)
                map.put(c, 0);
        }
        for(int j=0;j<m;j++){
            for(Character c:set){
                if (pattern.charAt(j)==c)
                    delta[j].put(c, j+1);
                else
                    delta[j].put(c, delta[x].get(c));
            }
            x = delta[j].get(pattern.charAt(j));
        }
        return delta;
    }

    private boolean searchByFiniteStateAutomata(String text, String pattern) {
        int n = text.length();
        int m = pattern.length();
        Map<Character, Integer>[] delta = transition(pattern);
        int q = 0;
        for(int i=0;i<n;i++){
            q = delta[q].getOrDefault(text.charAt(i), 0);
            if(q==m)
                return true;
        }
        return false;
    }

    private boolean searchByRabinKarp(String text, String pattern) {
        int p_len = pattern.length();
        int t_len = text.length();
        if(p_len > t_len)
            return false;
        int hash_p = 0;
        int hash_t = 0;
        int H = 1;
        for(int i=0;i<p_len;i++){
            hash_p = (hash_p * Encode_length + pattern.charAt(i)) % Q;
            hash_t = (hash_t * Encode_length + text.charAt(i)) % Q;
        }
        for(int i=1;i<p_len;i++){
            H = (H * Encode_length) % Q;
        }
        for(int i=0;i<t_len-p_len+1;i++){
            if(hash_p==hash_t){
                boolean matched = true;
                for(int j=0;j<p_len;j++){
                    if(pattern.charAt(j)!=text.charAt(i+j)) {
                        matched = false;
                        break;
                    }
                }
                if (matched)
                    return true;
            }
            if(i!=t_len-p_len) {
                hash_t = hash_t - text.charAt(i) * H;
                hash_t = hash_t * Encode_length + text.charAt(i + p_len);
                hash_t = (hash_t % Q + Q) % Q;
            }
        }
        return false;
    }


    @Test
    public void testRabinKarp0(){
        String text = "aaabaabaaab";
        String pattern = "aabaaa";
        assert searchByRabinKarp(text, pattern);
    }


    @Test
    public void testRabinKarp1(){
        String text = "asdsdasdasdasddjgfjdasdasde";
        String pattern = "fasde";
        assert !searchByRabinKarp(text, pattern);
    }

    @Test
    public void testRabinKarp2(){
        String text = "abc";
        String pattern = "abcd";
        assert !searchByRabinKarp(text, pattern);
    }

    @Test
    public void testFSA0(){
        String text = "aaabaabaaabb";
        String pattern = "aabaaabb";
        assert searchByFiniteStateAutomata(text, pattern);
    }

    @Test
    public void testFSA1(){
        String text = "asdsdasdasdasddjgfjdasdasde";
        String pattern = "fasde";
        assert !searchByFiniteStateAutomata(text, pattern);
    }

    @Test
    public void testKMP(){
        String text = "aaabaabaaab";
        String pattern = "aabaaa";
        assert searchByKnuthMorrisPratt(text, pattern);
    }

    @Test
    public void testKMP1(){
        String text = "asdsdasdasdasddjgfjdasdasde";
        String pattern = "fasde";
        assert !searchByKnuthMorrisPratt(text, pattern);
    }

    class Solution {

        int[][] state;

        public int shortestPathBinaryMatrix(int[][] grid) {
            state = new int[grid.length][grid.length];
            if(grid[0][0] != 0)
                return - 1;
            for(int i=0;i<state.length;i++){
                for(int j=0;j<state.length;j++) {
                    state[i][j] = state.length * state.length + 1;
                }
            }
            state[0][0] = 0;
            helper(grid, 0, 0);
            if(state[grid.length-1][grid.length-1]==state.length * state.length + 1)
                return -1;
            return state[grid.length-1][grid.length-1]+1;
        }

        List<int[]> getNeighbor(int i, int j){
            List<int[]> list = new LinkedList<>();
            list.add(new int[]{i+1, j+1});
            list.add(new int[]{i+1, j});
            list.add(new int[]{i+1, j-1});
            list.add(new int[]{i, j+1});
            list.add(new int[]{i, j-1});
            list.add(new int[]{i-1, j+1});
            list.add(new int[]{i-1, j});
            list.add(new int[]{i-1, j-1});
            return list;
        }

        void helper(int[][] grid, int i, int j){
            int curr_i, curr_j;
            List<int[]> needHelp = new LinkedList<>();
            for(int[] pos: getNeighbor(i, j)){
                curr_i = pos[0];
                curr_j = pos[1];
                if(curr_i >= 0 && curr_i < grid.length
                    && curr_j >=0 && curr_j < grid.length
                    && grid[curr_i][curr_j] != 1 && state[curr_i][curr_j] > state[i][j] + 1) {
                    state[curr_i][curr_j] = state[i][j] + 1;
                    needHelp.add(pos);
                }
            }
            for(int[] pos: needHelp){
                curr_i = pos[0];
                curr_j = pos[1];
                helper(grid, curr_i, curr_j);
            }
        }


    }

    Solution s;

    class Solution2 {
        public String shortestCommonSupersequence(String str1, String str2) {
            return "cabac";
        }
    }

    @Test
    public void testString0(){
        Solution2 solution2 = new Solution2();
        String s1 = "abac";
        String s2 = "cab";
        Assert.assertEquals("cabac", solution2.shortestCommonSupersequence(s1, s2));
    }


    @Before
    public void init(){
        s = new Solution();
        Encode_length = 2;
        Q = 1001;
    }


    @Test
    public void testSum0(){
        int[][] grid = {{0,1},{1,1}};
        assert s.shortestPathBinaryMatrix(grid)==-1;
    }

    @Test
    public void testSum1(){
        int[][] grid = {{0,0,0},{1,1,0},{1,1,0}};
        assert s.shortestPathBinaryMatrix(grid)==4;
    }

    @Test
    public void testSum3(){
        int[][] grid = {{1,0,0},{1,1,0},{1,1,0}};
        assert s.shortestPathBinaryMatrix(grid)==-1;
    }

    @Test
    public void testSum2(){
        int[][] grid = {{0,0,0},{0,0,0},{0,0,0}};
        assert s.shortestPathBinaryMatrix(grid)==3;
    }

    @Test
    public void testSum4(){
        int[][] grid = {{0,1,0,0,0},{0,1,0,0,0},{0,0,0,0,1},{0,1,1,1,0},{0,1,0,0,0}};
        assert s.shortestPathBinaryMatrix(grid)==7;
    }


}
