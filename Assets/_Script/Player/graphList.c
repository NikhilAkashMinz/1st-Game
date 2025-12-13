#include<stdio.h>
#include<stdlib.h>

typedef struct AdjNode{
    int dest
    struct AdjNode* next;
}*Node;

typedef struct AdjList{
    Node head;
}* List;

typedef struct myGraph{
    int verticies;
    List array;

}*Graph

Node AddNode(int dest)
{
    Node newNode = (Node)malloc(sizeof(AdjNode));
    newNode->dest = dest;
    newNode->next = NULL;

    return newNode;
}

Graph CreateGraph(int v)
{
    Graph Graph = (Graph)malloc(sizeof(myGraph));
    graph->verticies = v;

    graph->array = (List)malloc(sizeof(AdjList));

    for(int i = 0; i<graph->verticies;i++)
    {
        graph->array[i].head = NULL;
    }

    return graph;

}

void AddEdge(Graph graph, int src,int dest)
{
    Node newNode = AddNode(dest);
    newNode->next = graph->array[dest].head;
    graph->array[destc].head = newNode;

    Node newNode = AddNode(src);
    newNode->next = graph->array[src].head;
    graph->array[src].head = newNode;
}

void Display(Graph graph)
{
    for(int v=0; v< graph->verticies;i++)
    {
        Node temp = graph->array[v].head;
        printf("Adjacency of vertices %d \n head",v);

        while(temp)
        {
            printf("->%d", temp->dest);
            temp = temp->next;
        }
        printf("\n");
    }
}

void freeGraph(Graph graph)
{
    Node head,temp;
    if(graph)
    {
        if(graph->array)
        {
            for(int v = 0; v < graph->numVerticies;v++)
            {
                head = graph->array[v].head;
                temp = NULL;
                while(head)
                {
                    temp=head;
                    head=head->next;
                    free(temp);
                }
            }
            free(graph->array);
        }
        free(graph); 
    }
}

int mian()
{




    return 0;
}