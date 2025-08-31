import React, { useEffect, useState } from 'react';
import { useAppDispatch, useAppSelector } from '../../hooks';
import { fetchPosts } from './postsSlice';
import { PostItem } from '../../components/PostItem';
import { PostForm } from '../../components/PostForm';
import { Post } from './postsAPI';

export const Posts: React.FC = () => {
    const dispatch = useAppDispatch();
    const { posts, loading, error } = useAppSelector(state => state.posts);
    const [editingPost, setEditingPost] = useState<Post | null>(null);

    useEffect(() => {
        dispatch(fetchPosts());
    }, [dispatch]);

    return (
        <div className="max-w-2xl mx-auto p-4">
            <h1 className="text-2xl font-bold mb-4">Post Manager</h1>
            <PostForm editingPost={editingPost} clearEditing={() => setEditingPost(null)} />
            {loading && <p>Loading...</p>}
            {error && <p className="text-red-500">Error: {error}</p>}
            {posts.map(post => (
                <PostItem key={post.id} post={post} onEdit={setEditingPost} />
            ))}
        </div>
    );
};
