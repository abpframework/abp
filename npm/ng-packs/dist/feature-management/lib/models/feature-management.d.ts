export declare namespace FeatureManagement {
    interface State {
        features: Feature[];
    }
    interface ValueType {
        name: string;
        properties: object;
        validator: object;
    }
    interface Feature {
        name: string;
        value: string;
        description?: string;
        valueType?: ValueType;
        depth?: number;
        parentName?: string;
    }
    interface Features {
        features: Feature[];
    }
    interface Provider {
        providerName: string;
        providerKey: string;
    }
}
