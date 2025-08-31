import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import { fetchPostsAPI, createPostAPI, updatePostAPI, deletePostAPI } from './postsAPI';
import { Post, NewPost, PostsState } from './postsAPI';
import { RootState } from '../../app/store';

const initialState: PostsState = {
    posts: [],
    loading: false,
    error: null,
};

// GET
export const fetchPosts = createAsyncThunk<Post[]>(
    'posts/fetchPosts',
    async () => {
        return await fetchPostsAPI();
    }
);

// POST с ручной заменой id
export const createPost = createAsyncThunk<Post, NewPost, { state: RootState }>(
    'posts/createPost',
    async (newPost, { getState }) => {
        const created = await createPostAPI(newPost);
        const all = getState().posts.posts;
        const maxId = all.length ? Math.max(...all.map(p => p.id)) : 0;
        // возвращаем новый объект с уникальным id
        return { ...created, id: maxId + 1 };
    }
);

// PUT
export const updatePost = createAsyncThunk<Post, Post>(
    'posts/updatePost',
    async (post) => {
        return await updatePostAPI(post);
    }
);

// DELETE
export const deletePost = createAsyncThunk<number, number>(
    'posts/deletePost',
    async (id) => {
        await deletePostAPI(id);
        return id;
    }
);

export const postsSlice = createSlice({
    name: 'posts',
    initialState,
    reducers: {},
    extraReducers: builder => {
        builder
            .addCase(fetchPosts.pending, state => { state.loading = true; })
            .addCase(fetchPosts.fulfilled, (state, action: PayloadAction<Post[]>) => {
                state.loading = false;
                state.posts = action.payload;
            })
            .addCase(fetchPosts.rejected, (state, action) => {
                state.loading = false;
                state.error = action.error.message || 'Fetch error';
            })

            .addCase(createPost.fulfilled, (state, action: PayloadAction<Post>) => {
                state.posts.unshift(action.payload);
            })

            .addCase(updatePost.fulfilled, (state, action: PayloadAction<Post>) => {
                const idx = state.posts.findIndex(p => p.id === action.payload.id);
                if (idx !== -1) state.posts[idx] = action.payload;
            })

            .addCase(deletePost.fulfilled, (state, action: PayloadAction<number>) => {
                state.posts = state.posts.filter(p => p.id !== action.payload);
            });
    },
});

export default postsSlice.reducer;
