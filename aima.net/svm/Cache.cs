﻿namespace aima.net.svm
{
    //
    // Kernel Cache
    //
    // l is the number of total data items
    // size is the cache size limit in bytes
    //
    internal class Cache
    {
        private readonly int l;
        private long size;
        private class head_t
        {
            public head_t prev, next;  // a cicular list
            public float[] data;
            public int len;        // data[0,len) is cached in this entry
        }
        private readonly head_t[] head;
        private head_t lru_head;

        public Cache(int l_, long size_)
        {
            l = l_;
            size = size_;
            head = new head_t[l];
            for (int i = 0; i < l; i++) head[i] = new head_t();
            size /= 4;
            size -= l * (16 / 4);   // sizeof(head_t) == 16
            size = System.Math.Max(size, 2 * (long)l);  // cache must be large enough for two columns
            lru_head = new head_t();
            lru_head.next = lru_head.prev = lru_head;
        }

        private void lru_delete(head_t h)
        {
            // delete from current location
            h.prev.next = h.next;
            h.next.prev = h.prev;
        }

        private void lru_insert(head_t h)
        {
            // insert to last position
            h.next = lru_head;
            h.prev = lru_head.prev;
            h.prev.next = h;
            h.next.prev = h;
        }

        // request data [0,len)
        // return some position p where [p,len) need to be filled
        // (p >= len if nothing needs to be filled)
        // java: simulate pointer using single-element array
        public virtual int get_data(int index, float[][] data, int len)
        {
            head_t h = head[index];
            if (h.len > 0) lru_delete(h);
            int more = len - h.len;

            if (more > 0)
            {
                // free old space
                while (size < more)
                {
                    head_t old = lru_head.next;
                    lru_delete(old);
                    size += old.len;
                    old.data = null;
                    old.len = 0;
                }

                // allocate new space
                float[] new_data = new float[len];
                if (h.data != null) System.Array.Copy(h.data, 0, new_data, 0, h.len);
                h.data = new_data;
                size -= more;
                { int _ = h.len; h.len = len; len = _; }
            }

            lru_insert(h);
            data[0] = h.data;
            return len;
        }

        public virtual void swap_index(int i, int j)
        {
            if (i == j) return;

            if (head[i].len > 0) lru_delete(head[i]);
            if (head[j].len > 0) lru_delete(head[j]);
            { float[] _ = head[i].data; head[i].data = head[j].data; head[j].data = _; }
            { int _ = head[i].len; head[i].len = head[j].len; head[j].len = _; }
            if (head[i].len > 0) lru_insert(head[i]);
            if (head[j].len > 0) lru_insert(head[j]);

            if (i > j) { int _ = i; i = j; j = _; }
            for (head_t h = lru_head.next; h != lru_head; h = h.next)
            {
                if (h.len > i)
                {
                    if (h.len > j)
                    { float _ = h.data[i]; h.data[i] = h.data[j]; h.data[j] = _; }
                    else
                    {
                        // give up
                        lru_delete(h);
                        size += h.len;
                        h.data = null;
                        h.len = 0;
                    }
                }
            }
        }
    }
}
