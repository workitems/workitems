
export interface BooleanClause {
    type: string;
}

// property == value, Type == value, Id == value, Project == values
export interface PropertyEqualityClause extends BooleanClause {
    type: "PropertyEquality";
    propertyName: string;
    propertyValue: string;
}
export interface ProjectCodeEqualityClause extends BooleanClause {
    type: "ProjectCodeEquality";
    projectCode: string;
}
export interface IdEqualityClause extends BooleanClause {
    type: "IdEquality";
    id: string;
}
export interface WorkItemTypeEqualityClause extends BooleanClause {
    type: "WorkItemTypeEquality";
    workItemType: string;
}

// and, or, not
export interface BooleanMultiClause extends BooleanClause {
    subClauses: BooleanClause[];
    type: "And" | "Or";
}
export interface AndClause extends BooleanMultiClause {
    type: "And";
}
export interface OrClause extends BooleanMultiClause {
    type: "Or";
}
export interface NotClause extends BooleanClause {
    type: "Not";
}


export interface WorkItemQuery {
    editable: boolean;
    queryType: "List" | "Tree";
}

export interface WorkItemListQuery extends WorkItemQuery {
    clause: BooleanClause;
    queryType: "List";
}

export interface WorkItemTreeQuery extends WorkItemQuery {
    clauseHierarchy: BooleanClause[];
    queryType: "Tree";
}