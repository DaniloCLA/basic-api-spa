/* SystemJS module definition */
declare var module: NodeModule;
interface NodeModule {
  id: string;
}

interface JQuery {
  (selector: any): any;
  stellar(options?: any): any;
}