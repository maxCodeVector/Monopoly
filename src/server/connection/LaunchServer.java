package server.connection;

import server.base.Action;
import server.base.ServerApplication;

import java.io.File;
import java.io.FileFilter;
import java.io.IOException;
import java.net.URL;
import java.net.URLDecoder;
import java.util.*;

public class LaunchServer {

    static ClassLoader classLoader = new ClassLoader() {
        @Override
        public Class<?> loadClass(String name) throws ClassNotFoundException {
            return super.loadClass(name);
        }
    };

    public static void launch(Class cls, String[] args) {
        if(!cls.isAnnotationPresent(ServerApplication.class)){
            return;
        }
        List<Class> actionClass = new LinkedList<>();
        try {
            Set<Class<?>> res;
            res = getClasses(cls.getPackage().getName());
            for(Class one_lass: res){
                if(one_lass.isAnnotationPresent(Action.class)){
                    actionClass.add(one_lass);
                }
            }
            for (Class aClass : actionClass) {
                InstanceAfter(aClass);
                System.out.printf("Register action class %s\n", aClass.getName());
            }
        } catch (Exception e) {
            e.printStackTrace();
            System.err.println("Can not lauch server");
            return;
        }
        ServeNet.getInstance().start();
    }

    private static void InstanceAfter(Class z) throws IllegalAccessException, InstantiationException {
        ServeNet serveNet = ServeNet.getInstance();
        Object o = z.newInstance();
        if(o instanceof AfterDoingThis) {
            serveNet.afterInstance((AfterDoingThis) o);
        }else if(o instanceof Event){
            String protocolType = ((Action)z.getAnnotation(Action.class)).value();
            serveNet.addEvent(protocolType, (Event) o);
        }
    }
    /**
     * 从包package中获取所有的Class
     *
     * @param packageName the package need to scan
     * @return a set of class in this package
     */
    private static Set<Class<?>> getClasses(String packageName) throws Exception {

        Set<Class<?>> classes = new HashSet<>();
        String packageDirName = packageName.replace('.', '/');
        // 定义一个枚举的集合 并进行循环来处理这个目录下的things
        Enumeration<URL> dirs;
        try {
            dirs = Thread.currentThread().getContextClassLoader().getResources(packageDirName);
            while (dirs.hasMoreElements()) {
                URL url = dirs.nextElement();
                String protocol = url.getProtocol();
                if ("file".equals(protocol)) {
                    // 获取包的物理路径
                    String filePath = URLDecoder.decode(url.getFile(), "UTF-8");
                    addClass(classes, filePath, packageName);
                }
            }
        } catch (IOException e) {
            e.printStackTrace();
        }

        return classes;
    }

    private static void addClass(Set<Class<?>> classes, String filePath, String packageName) throws Exception {
        File[] files = new File(filePath).listFiles(new FileFilter() {
            @Override
            public boolean accept(File file) {
                return (file.isFile() && file.getName().endsWith(".class")) || file.isDirectory();
            }
        });
        for (File file : files) {
            String fileName = file.getName();
            if (file.isFile()) {
                String classsName = packageName+"."+fileName.substring(0, fileName.lastIndexOf("."));
                classes.add(classLoader.loadClass(classsName));

            }

        }
    }


}
