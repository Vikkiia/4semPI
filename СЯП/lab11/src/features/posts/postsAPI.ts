import axios from 'axios';

export interface Post {
    id: number;
    title: string;
    body: string;
}

export interface NewPost {
    title: string;
    body: string;
}

export interface PostsState {
    posts: Post[];
    loading: boolean;
    error: string | null;
}

const API_URL = 'https://jsonplaceholder.typicode.com/posts';

export const fetchPostsAPI = async (): Promise<Post[]> => {
    const { data } = await axios.get<Post[]>(API_URL);
    return data;
};

export const createPostAPI = async (newPost: NewPost): Promise<Post> => {
    const { data } = await axios.post<Post>(API_URL, newPost);
    return {
        ...data,
        id: Date.now(),
    };
};

export const updatePostAPI = async (post: Post): Promise<Post> => {
    // Если пост локальный (id > 100), пропускаем PUT к API, возвращаем сам объект
    if (post.id > 100) {
        return post;
    }

    try {
        const { data } = await axios.put<Post>(
            `${API_URL}/${post.id}`,
            post
        );
        return data;
    } catch (error) {
        console.warn(`PUT ${API_URL}/${post.id} failed, using local update.`);
        return post;
    }
};

export const deletePostAPI = async (id: number): Promise<void> => {
    // Если пост локальный (id > 100), пропускаем DELETE к API
    if (id > 100) {
        return;
    }

    try {
        await axios.delete(`${API_URL}/${id}`);
    } catch (error) {
        console.warn(`DELETE ${API_URL}/${id} failed, removing locally.`);
    }
};
