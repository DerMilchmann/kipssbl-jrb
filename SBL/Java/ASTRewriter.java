package fh.scheme.parser;

import fh.scheme.list.BooleElement;
import fh.scheme.list.Element;
import fh.scheme.list.EmptyElement;
import fh.scheme.list.NumberElement;
import fh.scheme.list.SchemeList;
import fh.scheme.list.StringElement;
import fh.scheme.list.TokenElement;

public class ASTRewriter {
	private SchemeList sl;
	private SchemeList current;
	private boolean quoted;

	public ASTRewriter() {
		sl = null;
		current = null;
		quoted = false;
	}

	public Element toSchemeList(Entry ast) {
		if (ast != null) {
			Entry e = null;
			Element neu = null;
			SchemeList save = current;
			if (ast.isLeaf()) {
				if ((ast.getToken().getType() == SchemeLexer.LPARENTHESIS) && quoted) {
					neu = new EmptyElement();
					quoted = false;
				} else {
					
					switch (ast.getToken().getType()) {
					case SchemeLexer.NUMBER:
						neu = new NumberElement(ast.getToken().getText());
						break;
					case SchemeLexer.BOOLEAN:
						neu = new BooleElement(ast.getToken().getText());
						break;
					case SchemeLexer.STRING:
						neu = new StringElement(ast.getToken().getText());
						break;
					case SchemeLexer.ELEMENT:
					case SchemeLexer.OPERATOR:
						neu = new TokenElement(ast.getToken().getText());
						break;
					case SchemeLexer.QUOTE:
						neu = new TokenElement("quote");
						quoted = true;
						break;
					}
				}
				if (current == null) {
					return neu;
				} else {
					if (neu != null) {
						current.append(neu);
					}
				}
			} else {
				current = new SchemeList();
				while ((e = ast.next()) != null) {
					toSchemeList(e);
				}
				if (save != null) {
					save.append(current);
					current = save;
				} else {
					sl = current;
				}

			}
		}
		return sl;
	}
}
