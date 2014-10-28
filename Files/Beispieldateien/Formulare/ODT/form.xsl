<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:draw="urn:oasis:names:tc:opendocument:xmlns:drawing:1.0" xmlns:text="urn:oasis:names:tc:opendocument:xmlns:text:1.0" xmlns:form="urn:oasis:names:tc:opendocument:xmlns:form:1.0">
  <xsl:output indent="yes" encoding="ISO-8859-1" method="xml" />

  <xsl:variable name="QUESTION" select = "'Frage'" />

  <xsl:template match="text:p[not(.//draw:control)]">
    <xsl:choose>
      <xsl:when test="starts-with(text(),$QUESTION)">
        <xsl:variable name="questionNr" select="count(preceding-sibling::text:p[starts-with(text(),$QUESTION)])+1" />
        <question nr="{$questionNr}">
          <xsl:value-of select="." />
        </question>
      </xsl:when>
      <xsl:otherwise>
        <p>
          <xsl:value-of select="." />
        </p>
      </xsl:otherwise>
     </xsl:choose>
  </xsl:template>

  <xsl:template match="form:checkbox">
    <checkbox id = "{@form:id}"  label="{@form:label}"></checkbox>
  </xsl:template>

  <xsl:template match="text:p[.//draw:control]">
    <xsl:variable name="controlId" select=".//draw:control/@draw:control" />
    <xsl:variable name="questionNr" select="count(preceding-sibling::text:p[starts-with(text(),$QUESTION)])" />
    <xsl:variable name="question" select="//text:p[starts-with(text(),$QUESTION)][$questionNr]" />
    <xsl:if test = "$questionNr &gt; 0" >
      <answer questionNr ="{$questionNr}">
        <xsl:apply-templates select="//form:checkbox[@form:id=$controlId]" />
      </answer>
    </xsl:if>
  </xsl:template>

  <xsl:template match="text:h">
    <h1>
      <xsl:value-of select="." />
    </h1>
  </xsl:template>

  <xsl:template match="text()|@*"> 
    <!-- ignore unmatched text -->
  </xsl:template>

  <xsl:template match="*">
    <!-- ignore unmatched nodes -->
  </xsl:template>

  <xsl:template match="/*">
    <form>
      <xsl:apply-templates select=".//text:h" />
      <xsl:apply-templates select=".//text:p" />
    </form>
  </xsl:template>
</xsl:stylesheet>
